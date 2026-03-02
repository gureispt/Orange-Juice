import React, { useState } from 'react';
import api from '../services/api';
import './ModalDeposito.css';

function ModalSaque({ conta, onClose, onSuccess }) {

    const [valor, setValor] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const handleSacar = async (e) => {
        e.preventDefault();
        setLoading(true);

        const valorNumerico = parseFloat(valor);

        if (valorNumerico <= 0) {
            setError('Valor deve ser maior que zero');
            setLoading(false);
            return;
        }

        if (valorNumerico > conta.saldo) {
            setError('Saldo insuficiente');
            setLoading(false);
            return;
        }

        try {
            await api.post(`Conta/${conta.id}/sacar`, valorNumerico, {
                headers: { 'Content-Type': 'application/json' }
            });

            alert('Saque realizado com sucesso!');
            onSuccess();
            onClose()
        } catch (_err) {
            setError(_err.response?.data || 'Eror ao realizar saque');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                <h2>💸 Sacar</h2>
                <p className="modal-conta-info">
                    {conta.tipoConta === 1 ? 'Conta Corrente' : 'Conta Investimento'}
                </p>
                <p className="modal-saldo">
                    Saldo atual: R$ {conta.saldo.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })}
                </p>

                <form onSubmit={handleSacar}>
                    <input
                        type="number"
                        step="0.01"
                        placeholder="Valor do saque"
                        value={valor}
                        onChange={(e) => setValor(e.target.value)}
                        required
                        className="modal-input"
                    />

                    {error && <p className="modal-error">{error}</p>}

                    <div className="modal-buttons">
                        <button type="button" onClick={onClose} className="modal-btn-cancel">
                            Cancelar
                        </button>
                        <button type="submit" disabled={loading} className="modal-btn-confirm">
                            {loading ? 'Processando...' : 'Sacar'}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}
export default ModalSaque;