import React, { useState } from 'react';
import api from '../services/api';
import './ModalDeposito.css';

function ModalDeposito({ conta, onClose, onSuccess }) {
    const [valor, setValor] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const handleDepositar = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError('');

        const valorNumerico = parseFloat(valor);

        if (valorNumerico <= 0) {
            setError('Valor deve ser maior que zero');
            setLoading(false);
            return;
        }

        try {
            await api.post(`/Conta/${conta.id}/depositar`, valorNumerico, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            alert('Depósito realizado com sucesso!');
            onSuccess();
            onClose();
        } catch (err) {
            setError(err.response?.data || 'Erro ao realizar depósito');
        } finally {
            setLoading(false);
        }
    };

    return (
         <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <h2>💰 Depositar</h2>
        <p className="modal-conta-info">
          {conta.tipoConta === 1 ? 'Conta Corrente' : 'Conta Investimento'}
        </p>
        <p className="modal-saldo">
          Saldo atual: R$ {conta.saldo.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}
        </p>

        <form onSubmit={handleDepositar}>
          <input
            type="number"
            step="0.01"
            placeholder="Valor do depósito"
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
              {loading ? 'Processando...' : 'Depositar'}
            </button>
          </div>
        </form>
      </div>
    </div>
    );
}

export default ModalDeposito;