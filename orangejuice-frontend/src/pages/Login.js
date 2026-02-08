import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../services/api';
import './Login.css';


function Login() {
    const [email, setEmail] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const navigate = useNavigate();
    
    const handleLogin = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError('');
    
        try {
            const response = await api.get(`/Usuario/email/${email}`);
    
            if (response.data) {
                localStorage.setItem('usuario', JSON.stringify(response.data));
                navigate('/dashboard');
            }
        } catch (err) {
            setError('Email não encontrado');
        } finally {
            setLoading(false);
        }
    };

    return (
    <div className="login-container">
      <div className="login-card">
        <h1 className="login-title">🍊 OrangeJuice Bank</h1>
        <p className="login-subtitle">Seu banco digital de investimentos</p>
        
        <form onSubmit={handleLogin} className="login-form">
          <input
            type="email"
            placeholder="Digite seu email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            className="login-input"
          />
          
          {error && <p className="login-error">{error}</p>}
          
          <button 
            type="submit" 
            disabled={loading}
            className="login-button"
          >
            {loading ? 'Entrando...' : 'Entrar'}
            </button>
            
            <p className="login-hint" style={{ marginTop: '15px', cursor: 'pointer' }}>
              Não tem conta ? <span
                onClick={() => navigate('/cadastrar')}
                style={{ color: '#667eea', fontWeight: 'bold', textDecoration: 'underline' }}>Cadastre-se
              </span>
            </p>
        </form>
      </div>
    </div>
  );
}
export default Login;