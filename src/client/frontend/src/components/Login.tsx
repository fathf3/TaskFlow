import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { apiService } from '../services/api';
import { useToast } from '@/hooks/use-toast';

const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();
  const { toast } = useToast();

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    try {
      const response = await apiService.login({ email, password });
      localStorage.setItem('token', response.token);
      localStorage.setItem('user', JSON.stringify(response.user));
      navigate('/'); // Dashboard'a yönlendir
    } catch (err: any) {
      setError('Giriş başarısız');
      toast({
        title: "Giriş başarısız",
        description: "Lütfen bilgilerinizi kontrol edin.",
        variant: "destructive",
      });
    }
  };

  return (
    <div style={{
      minHeight: '100vh',
      background: '#fff',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center'
    }}>
      <form
        onSubmit={handleLogin}
        style={{
          background: '#f9f9f9',
          padding: '2.5rem 2rem',
          borderRadius: 16,
          boxShadow: '0 4px 24px rgba(0,0,0,0.08)',
          minWidth: 320,
          display: 'flex',
          flexDirection: 'column',
          gap: 18
        }}
      >
        <h2 style={{ textAlign: 'center', marginBottom: 8, color: '#222' }}>Giriş Yap</h2>
        {error && <div style={{ color: '#e11d48', textAlign: 'center', fontSize: 15 }}>{error}</div>}
        <input
          type="email"
          placeholder="E-posta"
          value={email}
          onChange={e => setEmail(e.target.value)}
          required
          style={{
            padding: '12px',
            borderRadius: 8,
            border: '1px solid #d1d5db',
            fontSize: 16,
            outline: 'none'
          }}
        />
        <input
          type="password"
          placeholder="Şifre"
          value={password}
          onChange={e => setPassword(e.target.value)}
          required
          style={{
            padding: '12px',
            borderRadius: 8,
            border: '1px solid #d1d5db',
            fontSize: 16,
            outline: 'none'
          }}
        />
        <button
          type="submit"
          style={{
            background: 'linear-gradient(90deg, #2563eb 0%, #7c3aed 100%)',
            color: '#fff',
            border: 'none',
            borderRadius: 8,
            padding: '12px',
            fontSize: 16,
            fontWeight: 600,
            cursor: 'pointer',
            marginTop: 8,
            transition: 'background 0.2s'
          }}
        >
          Giriş Yap
        </button>
        <div style={{ textAlign: 'center', marginTop: 12 }}>
          <span>Hesabınız yok mu? </span>
          <Link to="/register" style={{ color: '#2563eb', fontWeight: 500, textDecoration: 'underline', cursor: 'pointer' }}>
            Kayıt Ol
          </Link>
        </div>
      </form>
    </div>
  );
};

export default Login;
