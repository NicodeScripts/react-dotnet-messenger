import React, { useState, useEffect } from 'react';
import './chatdropdown.css';
import { RiChatNewFill } from "react-icons/ri";
import Contact from "./contact";

const ChatDropdown = ({ userData, isOpen, onToggle, title, onNewConversation }) => {
  const [searchTerm, setSearchTerm] = useState('');
  const [activeUserName, setActiveUserName] = useState(null);

  const handleSearchChange = (e) => {
    setSearchTerm(e.target.value);
  };

  const handleUserClick = async (userName) => {
    setActiveUserName(prevUserName => (prevUserName === userName ? null : userName));
  };

  const handleAddUser = (userName) => {
    const token = localStorage.getItem('token');
    fetch(`https://localhost:5001/api/conversations/newconversation/${userName}`, {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json',
      },
    })
    .then(response => response.json())
    .then(data => {
      console.log('New conversation started:', data);
      if (onNewConversation) {
        onNewConversation();
      }
    })
    .catch(error => {
      console.error('Error starting conversation:', error);
    });
  };

  const handleSeeProfile = (userName) => {
    console.log(`Viewing profile for ${userName}`);
  };

  const filteredUsers = userData.filter(user =>
    user.userName.toLowerCase().includes(searchTerm.toLowerCase())
  );

  useEffect(() => {
    if (!isOpen) {
      setActiveUserName(null); 
    }
  }, [isOpen]);

  return (
    <div className="chat-dropdown-wrapper">
      <div className={`chat-dropdown-container ${isOpen ? 'open' : ''}`}>
        <div className="chat-icon-div" onClick={onToggle}>
          <RiChatNewFill className="chat-dropdown-icon" />
        </div>
        <div className={`chat-dropdown-menu ${isOpen ? 'open' : ''}`}>
          {title && <div className="chat-dropdown-title">{title}</div>}
          <input
            type="text"
            placeholder="Search"
            value={searchTerm}
            onChange={handleSearchChange}
            className="chat-dropdown-search"
          />
          <div className="conversation-list">
            {filteredUsers.map(user => (
              <Contact
                key={user.id}
                name={user.userName}
                preview={null}
                avatar={null}
                small
                isSelected={activeUserName === user.userName}
                onClick={() => handleUserClick(user.userName)}
                isChatDropdown
                showMenu={activeUserName === user.userName}
                onAddUser={() => handleAddUser(user.userName)}
                onSeeProfile={() => handleSeeProfile(user.userName)}
              />
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default ChatDropdown;
