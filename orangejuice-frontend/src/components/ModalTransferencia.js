import React, { useState } from 'react';
import api from '../services/api';
import './ModalDeposito.css';

function ModalTransferencia({ contas, onClose, onSuccess }) {
    const [tipoTransferencia, setTipoTransferencia] = useState('interna');
    const [contaOrigemId, setContaOrigemId] = useState('');
    const [contaDestinoId, setContaDestinoId] = useState('');
    const [numeroContaDestino, setNumeroContaDestino] = useState('');
    const [valor, setValor] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const contaOrigem = contas.find(c => c.id === contaOrigemId);

    const handleTransferir = async (e) => {
        e.preventDefault();
        setLoading(true);

        const valorNumerico = parseFloat(valor);

        if (valorNumerico <= 0) {
            setError('Valor deve ser maior que zero');
            setLoading(false);
            return;
        }

        if (!contaOrigemId) {
            setError('Selecione a conta de origem');
            setLoading(false);
            return;
        }

        try {
            if (tipoTransferencia === 'interna') {
                //TRANSFERENCIA ENTRE CONTAS DO MESMO USUÁRIO
                if (!contaDestinoId) {
                    setError('Selecione a conta de destino');
                    setLoading(false);
                    return;
                }

                if (contaOrigemId === contaDestinoId) {
                    setError('Contas de origem e destino devem ser diferentes');
                    setLoading(false);
                    return;
                }

                await api.post('/Conta/transferir-interna', {
                    contaOrigemId,
                    contaDestinoId,
                    valor: valorNumerico
                });
            } else {
                //TRANSFERENCIA EXTERNA
                if (!numeroContaDestino) {
                    setError('Informe o número da conta de destino');
                    setLoading(false);
                    return;
                }

                //BUSCA A CONTA DE DESTINO PELO NÚMERO
                const responseContaDestino = await api.get(`/Conta/numero/${numeroContaDestino}`);

                if (!responseContaDestino.data) {
                    setError('Conta destino não enotrada');
                    setLoading(false);
                    return;
                }

                await api.post('/Conta/trasnferir-externa', {
                    contaOrigemId,
                    contaDestinoId: responseContaDestino.data.id,
                    valor: valorNumerico
                });
            }

            alert('Trasnferência realizada com sucesso!');
            onSuccess();
            onClose();

        } catch (_err) {
            setError(_err.response?.data || 'Erro ao realizar transferência');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                <h2>🔄 Transferir</h2>

                {/* Tipo de transferência */}
                <div style={{ display: 'flex', gap: '10px', marginBottom: '15px' }}>
                    <button
                        type="button"
                        onClick={() => setTipoTransferencia('interna')}
                        className={tipoTransferencia === 'interna' ? 'modal-btn-confirm' : 'modal-btn-cancel'}
                        style={{ flex: 1, padding: '10px' }}
                    >
                        Entre minhas contas
                    </button>
                    <button
                        type="button"
                        onClick={() => setTipoTransferencia('externa')}
                        className={tipoTransferencia === 'externa' ? 'modal-btn-confirm' : 'modal-btn-cancel'}
                        style={{ flex: 1, padding: '10px' }}
                    >
                        Para outro usuário
                    </button>
                </div>

                <form onSubmit={handleTransferir}>
                    {/* Conta Origem */}
                    <select
                        value={contaOrigemId}
                        onChange={(e) => setContaOrigemId(e.target.value)}
                        required
                        className="modal-input"
                    >
                        <option value="">Selecione a conta de origem</option>
                        {contas.map(conta => (
                            <option key={conta.id} value={conta.id}>
                                {conta.tipoConta === 1 ? 'Conta Corrente' : 'Conta Investimento'} -
                                R$ {conta.saldo.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                            </option>
                        ))}
                    </select>

                    {/* Conta Destino - Interna */}
                    {tipoTransferencia === 'interna' && (
                        <select
                            value={contaDestinoId}
                            onChange={(e) => setContaDestinoId(e.target.value)}
                            required
                            className="modal-input"
                        >
                            <option value="">Selecione a conta de destino</option>
                            {contas
                                .filter(c => c.id !== contaOrigemId)
                                .map(conta => (
                                    <option key={conta.id} value={conta.id}>
                                        {conta.tipoConta === 1 ? 'Conta Corrente' : 'Conta Investimento'} -
                                        R$ {conta.saldo.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                                    </option>
                                ))}
                        </select>
                    )}

                    {/* Número da conta - Externa */}
                    {tipoTransferencia === 'externa' && (
                        <input
                            type="text"
                            placeholder="Número da conta de destino (ex: CC-123456)"
                            value={numeroContaDestino}
                            onChange={(e) => setNumeroContaDestino(e.target.value)}
                            required
                            className="modal-input"
                        />
                    )}

                    {/* Valor */}
                    <input
                        type="number"
                        step="0.01"
                        placeholder="Valor da transferência"
                        value={valor}
                        onChange={(e) => setValor(e.target.value)}
                        required
                        className="modal-input"
                    />

                    {/* Aviso de taxa para transferência externa */}
                    {tipoTransferencia === 'externa' && valor && (
                        <p style={{ color: '#666', fontSize: '13px', margin: '5px 0' }}>
                            ⚠️ Taxa de 0,5%: R$ {(parseFloat(valor) * 0.005).toFixed(2)}
                            {' '}| Total debitado: R$ {(parseFloat(valor) * 1.005).toFixed(2)}
                        </p>
                    )}

                    {/* Saldo disponível */}
                    {contaOrigem && (
                        <p className="modal-saldo">
                            Saldo disponível: R$ {contaOrigem.saldo.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
                        </p>
                    )}

                    {error && <p className="modal-error">{error}</p>}

                    <div className="modal-buttons">
                        <button type="button" onClick={onClose} className="modal-btn-cancel">
                            Cancelar
                        </button>
                        <button type="submit" disabled={loading} className="modal-btn-confirm">
                            {loading ? 'Processando...' : 'Transferir'}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}
export default ModalTransferencia;