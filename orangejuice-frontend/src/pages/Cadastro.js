import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';
import './Cadastro.css';

function Cadatro() {
    const [formData, setFormData] = useState({
        nome: '',
        email: '',
        cpf: '',
        senha: '',
        confirmaSenha: ''
    });
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleChange = (e) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value || ''
        })
    };

    const formatarCPF = (cpf) => {
        return cpf.replace(/\D/g, '');
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        console.log('1. Submit iniciado');
        // setError('');
        setLoading(true);
        
        console.log('2. Antes da validação');
        console.log('Senha:', formData.senha);
        console.log('Confirmar Senha:', formData.confirmaSenha);
        console.log('São iguais?', formData.senha === formData.confirmaSenha);
        
        //validações
        if (formData.senha !== formData.confirmaSenha) {
            console.log('3. ENTROU NO IF - senhas diferentes!');
            setError('As senhas não são iguais');
            setLoading(false);
            return;
        }

        console.log('4. Passou da validação de senha');

        if (formData.senha.length < 6) {
            console.log('5. ENTROU NO IF - senha curta!');

            setError('A senha deve ter no mínimo 6 caracteres');
            setLoading(false);
            return;
        }

        setError('');
        console.log('6. Passou de todas validações, chamando API');
        try {
            const response = await api.post('/Usuario/cadastrar', {
                nome: formData.nome,
                email: formData.email,
                cpf: formatarCPF(formData.cpf),
                senha: formData.senha
            });

            console.log('7. API respondeu:', response.data);
            if (response.data) {
                alert('Cadastro realizado com sucesso! Faça login para continuar.');
                navigate('/login');
            }
        } catch (error) {
            console.log('8. Erro na API:', error);
            setError(error.response?.data || 'Erro ao cadastrar. Tente novamente.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="cadastro-container">
            <div className="cadastro-card">
                <h1 className="cadastro-title">🍊 Criar Conta</h1>
                <p className="cadastro-subtitle">Cadastre-se no OrangeJuice Bank</p>
        
                <form onSubmit={handleSubmit} className="cadastro-form">
                    <input
                        type="text"
                        name="nome"
                        placeholder="Nome completo"
                        value={formData.nome}
                        onChange={handleChange}
                        required
                        className="cadastro-input"
                    />

                    <input
                        type="email"
                        name="email"
                        placeholder="Email"
                        value={formData.email}
                        onChange={handleChange}
                        required
                        className="cadastro-input"
                    />

                    <input
                        type="text"
                        name="cpf"
                        placeholder="CPF (apenas números)"
                        value={formData.cpf}
                        onChange={handleChange}
                        required
                        maxLength="11"
                        className="cadastro-input"
                    />

                    <input
                        type="password"
                        name="senha"
                        placeholder="Senha (mínimo 6 caracteres)"
                        value={formData.senha}
                        onChange={handleChange}
                        required
                        className="cadastro-input"
                    />

                    <input
                        type="password"
                        name="confirmaSenha"
                        placeholder="Confirmar senha"
                        value={formData.confirmaSenha}
                        onChange={handleChange}
                        required
                        className="cadastro-input"
                    />
          
                    {error && <p className="cadastro-error">{error}</p>}
          
                    <button
                        type="submit"
                        disabled={loading}
                        className="cadastro-button"
                    >
                        {loading ? 'Cadastrando...' : 'Cadastrar'}
                    </button>
                </form>

                <p className="cadastro-login">
                    Já tem conta? <span onClick={() => navigate('/login')}>Fazer login</span>
                </p>
            </div>
        </div>
    );
}
export default Cadatro;