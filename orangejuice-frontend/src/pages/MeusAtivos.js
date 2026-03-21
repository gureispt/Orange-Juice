import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';
import './MeusAtivos.css';

function MeusAtivos() {

    const [carteira, setCarteira] = useState([]);
    const [loading, setLoading] = useState(true);
    const [ativoSelecionado, setAtivoSelecionado] = useState(null);
    const [quantidadeVenda, setQuantidadeVenda] = useState('');
    const [loadingVenda, setLoadingVenda] = useState(false);
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const usuario = JSON.parse(localStorage.getItem('usuario'));

    useEffect(() => {
        if (!usuario) {
            navigate('/login')
            return;
        }
        carregarCarteira();
    }, []);

    const carregarCarteira = async () => {
        try {
            const response = await api.get(`/Carteira/usuario/${usuario.id}`);
            setCarteira(response.data);
        } catch (error) {
            console.error('Erro ao carregar carteira: ', error)
        } finally {
            setLoading(false);
        }
    }

    const getTipoLabel = (tipo) => {
        switch (tipo) {
            case 1: return 'Ação';
            case 2: return 'CDB';
            case 3: return 'Tesouro Direto';
            default: return 'Desconhecido'
        }
    };

    const getTipoClass = (tipo) => {
        switch (tipo) {
            case 1: return 'tipo-acao';
            case 2: return 'tipo-cdb';
            case 3: return 'tipo-tesouro';
            default: return '';
        }
    };


    const calcularIR = (lucro, tipoAtivo) => {
        if (lucro <= 0) return 0
        // AÇÕES 15% | CDB/TESOURO 22%
        const aliquota = tipoAtivo === 1 ? 0.15 : 0.22;
        return lucro * aliquota;
    };

    const calcularValorLiquido = () => {
        if (!ativoSelecionado || !quantidadeVenda) return 0;

        const valorBruto = ativoSelecionado.precoAtualAtivo * parseFloat(quantidadeVenda);
        const valorInvestido = ativoSelecionado.precoMedioCompra * parseFloat(quantidadeVenda);
        const lucro = valorBruto - valorInvestido;
        const ir = calcularIR(lucro, ativoSelecionado.tipoAtivo);

        return valorBruto - ir;
    };

    const handleVender = async () => {
        setError('');
        setLoadingVenda(true);

        if (!quantidadeVenda || parseFloat(quantidadeVenda) <= 0) {
            setError('Informe uma quantidade válida');
            setLoadingVenda(false);
            return;
        }

        if (parseFloat(quantidadeVenda) > ativoSelecionado.quantidade) {
            setError(`Você possui apenas ${ativoSelecionado.quantidade} unidades`);
            setLoadingVenda(false);
            return;
        }

        try {
            await api.post('Ativo/vender', {
                usuarioId: usuario.id,
                ativoId: ativoSelecionado.ativoId,
                quantidade: parseInt(quantidadeVenda)
            });
            alert(`Venda realizada! ${quantidadeVenda}X ${ativoSelecionado.codigoAtivo}`);
            setAtivoSelecionado(null);
            setQuantidadeVenda('');
            carregarCarteira();
        } catch (err) {
            setError(err.response?.data || 'Erro ao realizar venda');
        } finally {
            setLoadingVenda(false);
        }
    };
    
    const formatarMoeda = (valor) => {
        return valor.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
    };

    const formatarPercentual = (valor) => {
        const sinal = valor >= 0 ? '+' : '';
        return `${sinal}${valor.toFixed(2)}%`;
    }

    if (loading) return <div className="loading">Carregando...</div>

    return (
        <>
            <div className="meus-ativos-container">
                <header className="meus-ativos-header">
                    <button onClick={() => navigate('/dashboard')} className="back-btn">
                        ← Voltar
                    </button>
                    <h1>📊 Meus Ativos</h1>
                    <div></div>
                </header>

                <main className="meus-ativos-main">
                    {carteira.length === 0 ? (
                        <div className="empty-state">
                            <h3>Você ainda não possui ativos</h3>
                            <p>Comece investindo agora!</p>
                            <button 
                                onClick={() => navigate('/investir')} 
                                className="btn-investir"
                            >
                                📈 Investir
                            </button>
                        </div>
                    ) : (
                        <>
                            <div className="resumo-carteira">
                                <div className="resumo-item">
                                    <p>Total Investido</p>
                                    <h3>
                                        {formatarMoeda(
                                            carteira.reduce((acc, item) => acc + item.valorInvestido, 0)
                                        )}
                                    </h3>
                                </div>
                                <div className="resumo-item">
                                    <p>Valor Atual</p>
                                    <h3>
                                        {formatarMoeda(
                                            carteira.reduce((acc, item) => acc + item.valorAtual, 0)
                                        )}
                                    </h3>
                                </div>
                                <div className="resumo-item">
                                    <p>Lucro/Prejuízo</p>
                                    <h3 className={
                                        carteira.reduce((acc, item) => acc + item.lucroPrejuizo, 0) >= 0 
                                            ? 'lucro' 
                                            : 'prejuizo'
                                    }>
                                        {formatarMoeda(
                                            carteira.reduce((acc, item) => acc + item.lucroPrejuizo, 0)
                                        )}
                                    </h3>
                                </div>
                            </div>

                            <div className="ativos-lista">
                                {carteira.map((item) => (
                                    <div key={item.id} className="ativo-item">
                                        <div className="ativo-header">
                                            <div>
                                                <span className={`ativo-tipo-badge ${getTipoClass(item.tipoAtivo)}`}>
                                                    {getTipoLabel(item.tipoAtivo)}
                                                </span>
                                                <h3>{item.codigoAtivo}</h3>
                                                <p>{item.nomeAtivo}</p>
                                            </div>
                                            <button 
                                                onClick={() => {
                                                    setAtivoSelecionado(item);
                                                    setQuantidadeVenda('');
                                                    setError('');
                                                }}
                                                className="btn-vender"
                                            >
                                                Vender
                                            </button>
                                        </div>

                                        <div className="ativo-detalhes">
                                            <div className="detalhe-item">
                                                <span>Quantidade</span>
                                                <strong>{item.quantidade}</strong>
                                            </div>
                                            <div className="detalhe-item">
                                                <span>Preço Médio</span>
                                                <strong>{formatarMoeda(item.precoMedioCompra)}</strong>
                                            </div>
                                            <div className="detalhe-item">
                                                <span>Preço Atual</span>
                                                <strong>{formatarMoeda(item.precoAtualAtivo)}</strong>
                                            </div>
                                        </div>

                                        <div className="ativo-valores">
                                            <div className="valor-item">
                                                <span>Valor Investido</span>
                                                <strong>{formatarMoeda(item.valorInvestido)}</strong>
                                            </div>
                                            <div className="valor-item">
                                                <span>Valor Atual</span>
                                                <strong>{formatarMoeda(item.valorAtual)}</strong>
                                            </div>
                                            <div className="valor-item">
                                                <span>Lucro/Prejuízo</span>
                                                <strong className={item.lucroPrejuizo >= 0 ? 'lucro' : 'prejuizo'}>
                                                    {formatarMoeda(item.lucroPrejuizo)} 
                                                    ({formatarPercentual(item.percentualRetorno)})
                                                </strong>
                                            </div>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        </>
                    )}
                </main>
            </div>

            {/* Modal de Venda */}
            {ativoSelecionado && (
                <div className="modal-overlay" onClick={() => setAtivoSelecionado(null)}>
                    <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                        <h2>💰 Vender Ativo</h2>
                        
                        <div className="modal-ativo-info">
                            <span className={`ativo-tipo-badge ${getTipoClass(ativoSelecionado.tipoAtivo)}`}>
                                {getTipoLabel(ativoSelecionado.tipoAtivo)}
                            </span>
                            <h3>{ativoSelecionado.codigoAtivo}</h3>
                            <p>{ativoSelecionado.nomeAtivo}</p>
                        </div>

                        <div className="modal-dados">
                            <p>Quantidade disponível: <strong>{ativoSelecionado.quantidade}</strong></p>
                            <p>Preço atual: <strong>{formatarMoeda(ativoSelecionado.precoAtualAtivo)}</strong></p>
                        </div>

                        <input
                            type="number"
                            placeholder="Quantidade a vender"
                            value={quantidadeVenda}
                            onChange={(e) => setQuantidadeVenda(e.target.value)}
                            max={ativoSelecionado.quantidade}
                            min="1"
                            className="modal-input"
                        />

                        {quantidadeVenda && parseFloat(quantidadeVenda) > 0 && (
                            <div className="venda-resumo">
                                <div className="resumo-linha">
                                    <span>Valor Bruto</span>
                                    <strong>
                                        {formatarMoeda(ativoSelecionado.precoAtualAtivo * parseFloat(quantidadeVenda))}
                                    </strong>
                                </div>
                                <div className="resumo-linha">
                                    <span>
                                        IR ({ativoSelecionado.tipoAtivo === 1 ? '15%' : '22%'} sobre lucro)
                                    </span>
                                    <strong>
                                        {formatarMoeda(
                                            calcularIR(
                                                (ativoSelecionado.precoAtualAtivo * parseFloat(quantidadeVenda)) - 
                                                (ativoSelecionado.precoMedioCompra * parseFloat(quantidadeVenda)),
                                                ativoSelecionado.tipoAtivo
                                            )
                                        )}
                                    </strong>
                                </div>
                                <div className="resumo-linha total">
                                    <span>Valor Líquido</span>
                                    <strong>{formatarMoeda(calcularValorLiquido())}</strong>
                                </div>
                            </div>
                        )}

                        {error && <p className="modal-error">{error}</p>}

                        <div className="modal-buttons">
                            <button 
                                onClick={() => setAtivoSelecionado(null)} 
                                className="modal-btn-cancel"
                            >
                                Cancelar
                            </button>
                            <button 
                                onClick={handleVender}
                                disabled={loadingVenda}
                                className="modal-btn-confirm"
                            >
                                {loadingVenda ? 'Processando...' : 'Confirmar Venda'}
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </>
    );
}
export default MeusAtivos;