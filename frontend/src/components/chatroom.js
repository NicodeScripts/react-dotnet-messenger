import React, { useState, useEffect } from 'react';
import './chatroom.css';
import Message from './message'; // Import your Message component

const Chatroom = ({ selectedConversation, currentUserId }) => {
  const [message, setMessage] = useState('');
  const [chatroomMessages, setChatroomMessages] = useState([]);
  const [refreshMessages, setRefreshMessages] = useState(false); // Track when a message is sent
  const conversationId = selectedConversation.id;

  // Fetch messages when the conversationId changes or when a message is sent
  useEffect(() => {
    if (conversationId) {
      fetch(`https://localhost:5001/api/conversations/${conversationId}/messages`)
        .then(response => {
          if (!response.ok) {
            throw new Error(`Error fetching messages: ${response.statusText}`);
          }
          return response.json();
        })
        .then(data => {
          console.log("Fetched Messages:", data);
          if (Array.isArray(data)) {
            setChatroomMessages(data);
          } else {
            console.error("Expected an array but got:", data);
            setChatroomMessages([]);
          }
        })
        .catch(error => {
          console.error('Error fetching messages:', error);
          setChatroomMessages([]);
        });
    }
  }, [conversationId, refreshMessages]); // Add refreshMessages as a dependency

  // Handle sending a message
  const handleSendMessage = async () => {
    const token = localStorage.getItem('token'); // Assuming token is stored in localStorage
    if (!message.trim()) return;

    const requestBody = {
      message: message,
      conversationId: selectedConversation.id,
    };

    try {
      const response = await fetch('https://localhost:5001/api/conversations/sendMessage', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`, // Add Authorization header
        },
        body: JSON.stringify(requestBody),
      });

      if (response.ok) {
        setMessage(''); // Clear the input after message is sent
        console.log('Message sent successfully!');
        setRefreshMessages(!refreshMessages); // Toggle the refresh state to trigger a message fetch
      } else {
        console.error('Failed to send message');
      }
    } catch (error) {
      console.error('Error sending message:', error);
    }
  };

  // Send message on Enter key press
  const handleKeyDown = (e) => {
    if (e.key === 'Enter') {
      handleSendMessage();
    }
  };

  return (
    <div className="chatroom-container">
      <div className='chatroom-navbar'>
        <div className='navbar-contact-preview'>
          <img src='' alt='Contact Preview' className='contact-preview-img' />
        </div>
        <h2 className='navbar-username'>{selectedConversation.title}</h2>
      </div>
      <div className='chat-content'>
        {chatroomMessages.map(msg => (
          <Message key={msg.id} message={msg} currentUserId={currentUserId} />
        ))}
      </div>
      <div className="chatbar">
        <input
          className='chatbar-input'
          spellCheck='false'
          type="text"
          placeholder='Type a message'
          value={message}
          onChange={(e) => setMessage(e.target.value)}  // Update state with input value
          onKeyDown={handleKeyDown}  // Send message on Enter key press
        />
      </div>
    </div>
  );
};

export default Chatroom;
