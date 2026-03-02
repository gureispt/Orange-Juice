import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';
import './Dashboard.css';
import ModalDeposito from '../components/ModalDeposito';
import ModalSaque from '../components/ModalSaque';
import ModalTransferencia from '../components/ModalTransferencia';

function Dashboard() {
  const [usuario, setUsuario] = useState(null);
  const [contas, setContas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [modalDeposito, setModalDeposito] = useState(null);
  const [modalSaque, setModalSaque] = useState(null);
  const [modalTransferencia, setModalTransferencia] = useState(false);

  const navigate = useNavigate();

  useEffect(() => {
    const usuarioLogado = JSON.parse(localStorage.getItem('usuario'));

    if (!usuarioLogado) {
      navigate('/login');
      return;
    }
    setUsuario(usuarioLogado);
    carregarContas(usuarioLogado.id);
  }, [navigate]);

  const carregarContas = async (usuarioId) => {
    try {
      const response = await api.get(`/Conta/usuario/${usuarioId}`)
      setContas(response.data);
    } catch (error) {
      console.error('Erro ao carregar contas: ', error);
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = () => {
    localStorage.removeItem('usuario');
    navigate('/login');
  };

  const getTipoConta = (tipo) => {
    return tipo === 1 ? 'Conta corrente' : 'Conta Investimento';
  };

  if (loading) return <div className="loading">Carregando...</div>;

  return (
    <>
      <div className="dashboard-container">
        <header className="dashboard-header">
          <h1>🍊 OrangeJuice Bank</h1>
          <div className="user-info">
            <span>Olá, {usuario?.nome}</span>
            <button onClick={handleLogout} className="logout-btn">Sair</button>
          </div>
        </header>

        <main className="dashboard-main">
          <h2>Minhas Contas</h2>

          <div className="contas-grid">
            {contas.map((conta) => (
              <div key={conta.id} className="conta-card">
                <div className="conta-header">
                  <h3>{getTipoConta(conta.tipoConta)}</h3>
                  <span className="conta-numero">{conta.numeroConta}</span>
                </div>
                <div className="conta-saldo">
                  <p>Saldo disponível</p>
                  <h2>R$ {conta.saldo.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</h2>
                </div>
                <div className="conta-actions">
                  <button className="action-btn" onClick={() => setModalDeposito(conta)}> Depositar </button>
                  <button className="action-btn" onClick={() => setModalSaque(conta)}> Sacar </button>
                  <button className="action-btn" onClick={() => setModalTransferencia(true)}> Transferir </button>
                </div>
                
              </div>
            ))}
          </div>

          <div className="quick-actions">
            <button className="quick-action-btn" onClick={() => navigate('/investir')}>📈 Investir</button>
            <button className="quick-action-btn">📊 Meus Ativos</button>
            <button className="quick-action-btn">📄 Relatórios</button>
          </div>
        </main>
      </div>

      {modalDeposito && (
        <ModalDeposito 
          conta={modalDeposito} 
          onClose={() => setModalDeposito(null)} 
          onSuccess={() => { carregarContas(usuario.id); setModalDeposito(null); }}
        />
      )}

      {modalSaque && (
        <ModalSaque
          conta={modalSaque}
          onClose={() => setModalSaque(null)}
          onSuccess={() => { carregarContas(usuario.id); setModalSaque(null); }}
        />
      )}

      {modalTransferencia && (
        <ModalTransferencia
          contas={contas}
          onClose={() => setModalTransferencia(false)}
          onSuccess={() => {
            carregarContas(usuario.id);
            setModalTransferencia(false);
          }}
        />
      )}
    </>
  );
}
      export default Dashboard;