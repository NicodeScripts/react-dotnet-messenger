import React, { useState, useEffect } from 'react';
import './contactslist.css';
import ConversationList from './conversationlist';
import Dropdown from './dropdown';
import ChatDropdown from './chatdropdown';
import { FaAngleDown } from 'react-icons/fa';

const ContactsList = ({ onConversationSelect }) => {
  const [selectedOption, setSelectedOption] = useState(null);
  const [selectedContact, setSelectedContact] = useState(null);
  const [openDropdown, setOpenDropdown] = useState(null);
  const [conversations, setConversations] = useState([]);
  const [usersWithoutConv, setUsersWithoutConv] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');

  const filterOptions = [
    { label: 'Recent', value: 'recent' },
    { label: 'Unread', value: 'unread' },
    { label: 'Known', value: 'known' },
  ];

  const fetchUsersWithoutConversation = async () => {
    const token = localStorage.getItem('token');
  
    try {
      const response = await fetch('https://localhost:5001/api/conversations/usersWithoutConversation', {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      });
  
      if (response.ok) {
        const users = await response.json();
        setUsersWithoutConv(users);
      } else {
        console.log('Failed to fetch users without conversation');
      }
    } catch (error) {
      console.error('An error occurred:', error);
    }
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

  useEffect(() => {
    fetchConversations();
    fetchUsersWithoutConversation();
  }, [selectedContact]);

  const handleConversationClick = (conversation) => {
    setSelectedContact(conversation.id);
    if (onConversationSelect) {
      onConversationSelect(conversation);
    }
  };

  const toggleChatDropdown = () => {
    setOpenDropdown((prev) => (prev === 'chat' ? null : 'chat'));
  };

  const toggleFilterDropdown = () => {
    setOpenDropdown((prev) => (prev === 'filter' ? null : 'filter'));
  };

  const handleNewConversation = () => {
    fetchConversations();
    fetchUsersWithoutConversation();
  };

  const handleSearchChange = (e) => {
    setSearchTerm(e.target.value.toLowerCase()); // Update the search term
  };

  // Filter conversations based on the search term
  const filteredConversations = conversations.filter((conversation) => {
    return conversation.title?.toLowerCase().includes(searchTerm);
  });

  return (
    <div className="contacts-container">
      <div className="top-container">
        <h1 className="chat-title">
          Chats
          <div className="dropdown-wrapper">
            <ChatDropdown 
              userData={usersWithoutConv}
              isOpen={openDropdown === 'chat'} 
              onToggle={toggleChatDropdown} 
              title="New Chat" 
              onNewConversation={handleNewConversation} // Pass the handler as a prop
            />
          </div>
        </h1>
        <input 
          type="text" 
          placeholder="Search Here" 
          className="search-container" 
          onChange={handleSearchChange} // Set search term on input change
        />
        <div className="top-middle-container">
          <Dropdown
            options={filterOptions}
            selected={selectedOption}
            setSelected={setSelectedOption}
            placeholder="Recent Chats"
            Icon={FaAngleDown}
            title={"Filter By"}
            isOpen={openDropdown === 'filter'}
            onToggle={toggleFilterDropdown}
          />
          <div className="contacts-list">
            <ConversationList 
              conversations={filteredConversations}
              onConversationClick={handleConversationClick}
              selectedContact={selectedContact}
            />
          </div>
        </div>
      </div>
    </div>
  );
};

export default ContactsList;
