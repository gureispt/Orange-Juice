import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import Cadastro from './pages/Cadastro';
import Investir from './pages/Investir';
import './App.css';
import MeusAtivos from './pages/MeusAtivos';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Navigate to="/login" />} />
        <Route path="/login" element={<Login />} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/cadastrar" element={<Cadastro />} />
        <Route path="/investir" element={<Investir />} />
        <Route path="/meus-ativos" element={<MeusAtivos />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;