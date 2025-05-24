import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './register.css';
import { FaRegEye, FaRegEyeSlash, FaSpinner } from "react-icons/fa"; // Reuse icons from login page
import { GoDotFill } from "react-icons/go"; // For consistent design

const Register = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false); // To toggle password visibility
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false); // For loading state
  const navigate = useNavigate();

  const togglePasswordVisibility = () => {
    setShowPassword(prevState => !prevState);
  };

  const handleRegister = async (e) => {
    e.preventDefault();
    setLoading(true); // Show loading spinner

    try {
      const response = await fetch('https://localhost:5001/api/auth/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ userName: username, password: password }),
      });

      if (response.ok) {
        navigate('/login'); // Navigate to login after successful registration
      } else {
        const errorData = await response.json();
        const errorMessages = [];

        if (errorData.errors) {
          for (let key in errorData.errors) {
            if (Array.isArray(errorData.errors[key])) {
              errorMessages.push(errorData.errors[key].join(' '));
            } else {
              errorMessages.push(errorData.errors[key]);
            }
          }
        } else if (errorData.message) {
          errorMessages.push(errorData.message);
        }

        setError('Registration failed: ' + errorMessages.join(' '));
      }
    } catch (error) {
      setError('An error occurred: ' + error.message);
    } finally {
      setLoading(false); // Hide loading spinner after request
    }
  };

  return (
    <div className="register-container">
      <form className="register-form" onSubmit={handleRegister}>
        <h2>Register</h2>
        <input
          type="text"
          placeholder="Username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          maxLength={32}
        />
        <div className="password-container">
          <input
            type={showPassword ? "text" : "password"}
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            maxLength={64}
          />
          <div className="icon-container" onClick={togglePasswordVisibility}>
            {showPassword ? <FaRegEye /> : <FaRegEyeSlash />}
          </div>
        </div>
        <button type="submit" disabled={loading}>
          {loading ? <FaSpinner className="spinner" /> : "Confirm"} {/* Loading spinner */}
        </button>
        {error && <div className="error-message">{error}</div>}
      </form>
      <div className="below-login">
        <a className="login-link" onClick={() => navigate('/login')}>Back to Login</a>
      </div>
    </div>
  );
};

export default Register;
