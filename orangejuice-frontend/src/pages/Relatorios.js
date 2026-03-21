import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';
import './Relatorios.css';

function Relatorios() {
    const [contas, setContas] = useState([]);
    const [transacoes, setTransacoes] = useState([]);
    const [carteira, setCarteira] = useState([]);
    const [contaSelecionada, setContaSelecionada] = useState(null);
    const [tipoRelatorio, setTipoRelatorio] = useState('extrato-cc');
    const [periodo, setPeriodo] = useState({
        inicio: new Date(new Date().getFullYear(), 0, 1).toISOString().split('T')[0], // 01/01 do ano atual
        fim: new Date().toISOString().split('T')[0] //HOJE
    });
    const [loading, setLoading] = useState(true);
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
            const [contasRes, transacoesRes, carteiraRes] = await Promise.all([
                api.get(`/Conta/usuario/${usuario.id}`),
                api.get(`/Transacao/usuario/${usuario.id}`),
                api.get(`/Carteira/usuario/${usuario.id}`)
            ]);

            setContas(contasRes.data);
            setTransacoes(transacoesRes.data);
            setCarteira(carteiraRes.data);

            const contaCorrente = contasRes.data.find(c => c.tipoConta === 1);
            setContaSelecionada(contaCorrente);
        } catch (err) {
            console.error('Erro ao carregar dados: ', err);
        }
    }
}