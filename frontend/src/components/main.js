import React, { useState, useEffect, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import './main.css';
import SideBar from './sidebar';
import ContactsList from './contactslist';
import CallsList from './callslist'; 
import Chatroom from './chatroom';
import SettingsMenu from './settingsmenu';

const MainPage = () => {
  const [selectedMode, setSelectedMode] = useState('chat');
  const [selectedConversation, setSelectedConversation] = useState(null);
  const [userName, setUserName] = useState('');
  const [users, setUsers] = useState([]);
  const [conversations, setConversations] = useState([null]);
  const [settingsIsOpen, setSettingsIsOpen] = useState(false);

  const navigate = useNavigate();

  const handleConversationSelect = (conversation) => {
    setSelectedConversation(conversation);
  };

  const toggleSettingsMenu = () => {
    setSettingsIsOpen((prev) => !prev);  // Toggles the settings menu
  };

  const closeSettingsMenu = () => {
    setSettingsIsOpen(false);  // Closes the settings menu
  };

  const fetchConversations = async () => {
    try {
      const token = localStorage.getItem('token');
      const response = await fetch('https://localhost:5001/api/conversations/currentuser', {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      });

      if (response.ok) {
        const data = await response.json();
        setConversations(data);
      } else {
        console.log('Failed to fetch conversations');
      }
    } catch (error) {
      console.error('An error occurred:', error);
    }
  };

  const fetchUserDetails = useCallback(async () => {
    const token = localStorage.getItem('token');
  
    if (!token) {
      console.log('No token found, redirecting to login.');
      navigate('/login');
      return;
    }

    try {
      const response = await fetch('https://localhost:5001/api/auth/me', {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      });
  
      if (response.ok) {
        const data = await response.json();
        setUserName(data.userName);
        console.log('Logged in as:', data.userName);
      } else {
        console.log(`Failed to fetch user details: ${response.status} - ${response.statusText}`);
        localStorage.removeItem('token');
        navigate('/login');
      }
    } catch (error) {
      console.error('An error occurred during fetch:', error);
      navigate('/login');
    }
  }, [navigate]);

  const fetchAllUsers = useCallback(async () => {
    const token = localStorage.getItem('token');
    if (!token) {
      navigate('/login');
      return;
    }

    try {
      const response = await fetch('https://localhost:5001/api/user/all', {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      });

      if (response.ok) {
        const usersData = await response.json();
        setUsers(usersData);
        console.log('List of users:', usersData);
      } else {
        console.log('Failed to fetch users');
      }
    } catch (error) {
      console.error('An error occurred:', error);
    }
  }, [navigate]);

  useEffect(() => {
    fetchUserDetails();
    fetchConversations();
  }, []);

  return (
    <div className="main-container">
      <div className="content">
        <SideBar selectedMode={selectedMode} setSelectedMode={setSelectedMode} onToggleSettingsMenu={toggleSettingsMenu} />
        <div className="chat-components">
          <div className="contacts-div">
            {selectedMode === 'chat' && (
              <ContactsList 
                selectedMode={selectedMode} 
                onConversationSelect={handleConversationSelect} 
              />
            )}
            {selectedMode === 'call' && (
              <CallsList 
                selectedMode={selectedMode} 
              />
            )}
          </div>
          <div className="chatroom-div">
            {selectedConversation && (
              <Chatroom 
                selectedConversation={selectedConversation} 
                currentUserId={userName} 
              />
            )}
          </div>
          <div className='settings-menu'>
            <SettingsMenu isOpen={settingsIsOpen} onClose={closeSettingsMenu} /> {/* Pass close function */}
          </div>
        </div>
      </div>
    </div>
  );
};

export default MainPage;
