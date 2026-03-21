import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';
import './Investir.css'

function Investir() {
    const [ativos, setAtivos] = useState([]);
    const [contaInvestimento, setContaInvestimento] = useState(null);
    const [filtroTipo, setFiltroTipo] = useState('todos');
    const [loading, setLoading] = useState(true);
    const [ativoSelecionado, setAtivoSelecionado] = useState(null);
    const [quantidade, setQuantidade] = useState('');
    const [loadingCompra, setLoadingCompra] = useState(false);
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const usuario = JSON.parse(localStorage.getItem('usuario'));

    useEffect(() => {
        if (!usuario) {
            navigate('/login');
            return;
        }
        carregarDados();
    }, []);

    const carregarDados = async () => {
        try {
            const [ativos, contas] = await Promise.all([
                api.get('/Ativo'),
                api.get(`/Conta/usuario/${usuario.id}`)
            ]);

            setAtivos(ativos.data);

            const ci = contas.data.find((c => c.tipoConta === 2));
            setContaInvestimento(ci);
        } catch (error) {
            console.error('Erro ao carregar dados: ', error)
        } finally {
            setLoading(false);
        }
    };

    const getTipoLabel = (tipo) => {
        switch (tipo) {
            case 1: return 'Ação';
            case 2: return 'CDB';
            case 3: return 'Tesouro Direto';
            default: return 'Desconhecido';
        }
    };

    const getTipoColor = (tipo) => {
        switch (tipo) {
            case 1: return '#667eea';
            case 2: return '#11998e';
            case 3: return '#f7971e';
            default: return '#999';
        }
    };

    const ativosFiltrados = ativos.filter(ativo => {
        if (filtroTipo === 'todos') return true;
        if (filtroTipo === 'acoes') return ativo.tipoAtivo === 1;
        if (filtroTipo === 'cdb') return ativo.tipoAtivo === 2;
        if (filtroTipo === 'tesouro') return ativo.tipoAtivo === 3;
        return true;
    });

    const calcularTotal = () => {
        if (!ativoSelecionado || !quantidade) return 0;
        const total = ativoSelecionado.precoAtual * parseFloat(quantidade);
        const taxa = ativoSelecionado.tipoAtivo === 1 ? total * 0.01 : 0
        return total + taxa;
    };

    const handleComprar = async () => {
        setError('');
        setLoadingCompra(true);

        if (!quantidade || parseFloat(quantidade) <= 0) {
            setError('Informe uma quantidade válida');
            setLoadingCompra(false);
            return;
        }

        const total = calcularTotal();

        if (total > contaInvestimento.saldo) {
            setError(`Saldo insuficiente! Necessário: R$ ${total.toFixed(2)}`);
            setLoadingCompra(false);
            return;
        }

        try {
            await api.post('/Ativo/comprar', {
                usuarioId: usuario.id,
                ativoId: ativoSelecionado.id,
                quantidade: parseInt(quantidade)
            });

            alert(`Compra realizada! ${quantidade}x ${ativoSelecionado.codigo}`);
            setAtivoSelecionado(null);
            setQuantidade('');
            carregarDados(); // Atualiza saldo
        } catch (err) {
            setError(err.response?.data || 'Erro ao realizar compra');
        } finally {
            setLoadingCompra(false);
        }
    };

    if (loading) return <div className="loading">Carregando...</div>;

    return (
        <div className="investir-container">
            <header className="investir-header">
                <button onClick={() => navigate('/dashboard')} className="back-btn">
                    ← Voltar
                </button>
                <h1>📈 Investir</h1>
                <div className="saldo-ci">
                    <p>Saldo disponível</p>
                    <h3>R$ {contaInvestimento?.saldo.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}</h3>
                </div>
            </header>

            <main className="investir-main">
                {/* Filtros */}
                <div className="investir-filtros">
                    {['todos', 'acoes', 'cdb', 'tesouro'].map(filtro => (
                        <button
                            key={filtro}
                            onClick={() => setFiltroTipo(filtro)}
                            className={`filtro-btn ${filtroTipo === filtro ? 'ativo' : ''}`}
                        >
                            {filtro === 'todos' ? 'Todos' :
                                filtro === 'acoes' ? '📊 Ações' :
                                    filtro === 'cdb' ? '🏦 CDB' : '🏛️ Tesouro'}
                        </button>
                    ))}
                </div>

                <div className="investir-content">
                    {/* Lista de Ativos */}
                    <div className="ativos-lista">
                        {ativosFiltrados.map(ativo => (
                            <div
                                key={ativo.id}
                                className={`ativo-card ${ativoSelecionado?.id === ativo.id ? 'selecionado' : ''}`}
                                onClick={() => {
                                    setAtivoSelecionado(ativo);
                                    setQuantidade('');
                                    setError('');
                                }}
                            >
                                <div className="ativo-info">
                                    <span
                                        className="ativo-tipo"
                                        style={{ background: getTipoColor(ativo.tipoAtivo) }}
                                    >
                                        {getTipoLabel(ativo.tipoAtivo)}
                                    </span>
                                    <h3>{ativo.codigo}</h3>
                                    <p>{ativo.nome}</p>
                                </div>
                                <div className="ativo-preco">
                                    <h3>R$ {ativo.precoAtual.toFixed(2)}</h3>
                                    {ativo.taxaRentabilidadeAnual && (
                                        <p className="rentabilidade">
                                            {ativo.taxaRentabilidadeAnual}% a.a.
                                        </p>
                                    )}
                                </div>
                            </div>
                        ))}
                    </div>

                    {/* Painel de Compra */}
                    {ativoSelecionado && (
                        <div className="compra-painel">
                            <h2>Comprar {ativoSelecionado.codigo}</h2>
                            <p className="compra-nome">{ativoSelecionado.nome}</p>
                            <p className="compra-preco">
                                Preço unitário: R$ {ativoSelecionado.precoAtual.toFixed(2)}
                            </p>

                            {ativoSelecionado.tipoAtivo === 1 && (
                                <p className="compra-taxa">⚠️ Taxa de corretagem: 1%</p>
                            )}

                            <input
                                type="number"
                                placeholder="Quantidade"
                                value={quantidade}
                                onChange={(e) => setQuantidade(e.target.value)}
                                className="modal-input"
                                min="1"
                            />

                            {quantidade && (
                                <div className="compra-resumo">
                                    <p>Subtotal: R$ {(ativoSelecionado.precoAtual * parseFloat(quantidade || 0)).toFixed(2)}</p>
                                    {ativoSelecionado.tipoAtivo === 1 && (
                                        <p>Taxa (1%): R$ {(ativoSelecionado.precoAtual * parseFloat(quantidade || 0) * 0.01).toFixed(2)}</p>
                                    )}
                                    <h3>Total: R$ {calcularTotal().toFixed(2)}</h3>
                                </div>
                            )}

                            {error && <p className="modal-error">{error}</p>}

                            <button
                                onClick={handleComprar}
                                disabled={loadingCompra}
                                className="comprar-btn"
                            >
                                {loadingCompra ? 'Processando...' : '✅ Confirmar Compra'}
                            </button>

                            <button
                                onClick={() => setAtivoSelecionado(null)}
                                className="cancelar-btn"
                            >
                                Cancelar
                            </button>
                        </div>
                    )}
                </div>
            </main>
        </div>
    );
}
export default Investir;