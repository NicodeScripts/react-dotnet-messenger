import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './login.css';
import { FaRegEye, FaRegEyeSlash, FaSpinner } from "react-icons/fa";
import { GoDotFill } from "react-icons/go";

const Login = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false); // State to manage password visibility
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  // Function to toggle password visibility
  const togglePasswordVisibility = () => {
    setShowPassword(prevState => !prevState);
  };

  const handleLogin = async (e) => {
    e.preventDefault();
    setLoading(true);
  
    try {
      const response = await fetch('https://localhost:5001/api/auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ userName: username, password: password }),
      });
  
      if (response.ok) {
        const data = await response.json();
        localStorage.setItem('token', data.token); // Store the JWT token
        // console.log(`Recognised ${username}, ${password}`);
        navigate('/main');
      } else {
        const errorData = await response.json();
        setError(errorData.message || 'Login failed.');
      }
    } catch (error) {
      console.error('An error occurred:', error);
      setError('An unexpected error occurred.');
    } finally {
      setLoading(false);
    }
  };
  

  return (
    <div className="login-container">
      <form className="login-form" onSubmit={handleLogin}>
        <h2>Login</h2>
        <input
          type="text"
          placeholder="Username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          maxLength={32}
        />
        <div className="password-container">
          <input
            type={showPassword ? "text" : "password"} // Toggle input type based on showPassword state
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            maxLength={64}
          />
          <div className="icon-container" onClick={togglePasswordVisibility}>
            {showPassword ? <FaRegEye /> : <FaRegEyeSlash />}
          </div>
        </div>
        <button type="submit" disabled={loading}> {/* Disable button while loading */}
          {loading ? <FaSpinner className="spinner" /> : "Login"} {/* Conditionally render spinner or text */}
        </button>
        {error && <div className="error-message">{error}</div>}
      </form>
      <div className="below-login">
        <a className='login-link' onClick={() => navigate('/register')}>Register an account</a>
        <GoDotFill className="go-dot-fill"/>
        <a className='login-link'>Forgot Password?</a>
      </div>
    </div>
  );
};

export default Login;
