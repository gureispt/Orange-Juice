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


}
export default MeusAtivos;