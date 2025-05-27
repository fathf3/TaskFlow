import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { apiService } from '../services/api';
import { useToast } from '@/hooks/use-toast';

const Register = () => {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();
  const { toast } = useToast();

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    try {
      await apiService.register({ firstName, lastName, email, password });

      toast({
        title: "Kayıt başarılı",
        description: "Giriş yapabilirsiniz.",
        variant: "default",
      });
      navigate('/login');
    } catch (err: any) {
      setError('Kayıt başarısız');
      toast({
        title: "Kayıt başarısız",
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
        onSubmit={handleRegister}
        style={{
          background: '#f9f9f9',
          padding: '2.5rem 2rem',
          borderRadius: 16,
          boxShadow: '0 4px 24px rgba(0,0,0,0.08)',
          minWidth: 340,
          display: 'flex',
          flexDirection: 'column',
          gap: 18
        }}
      >
        <h2 style={{ textAlign: 'center', marginBottom: 8, color: '#222' }}>Kayıt Ol</h2>
        {error && <div style={{ color: '#e11d48', textAlign: 'center', fontSize: 15 }}>{error}</div>}
        <input
          type="text"
          placeholder="Ad"
          value={firstName}
          onChange={e => setFirstName(e.target.value)}
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
          type="text"
          placeholder="Soyad"
          value={lastName}
          onChange={e => setLastName(e.target.value)}
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
          Kayıt Ol
        </button>
      </form>
    </div>
  );
};

export default Register;