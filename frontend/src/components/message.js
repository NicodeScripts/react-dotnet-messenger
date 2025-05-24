import React from 'react';
import './message.css'; // Import CSS for styling

const Message = ({ message, currentUserId }) => {
  const { id, message: messageContent, timestamp, sender } = message;

  // Format the timestamp (optional)
  const formatTimestamp = (timestamp) => {
    const date = new Date(timestamp);
    const currentDate = new Date();

    if (date.toDateString() === currentDate.toDateString()) {
      return `Today ${date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}`;
    }

    return `${date.getDate()}/${date.getMonth() + 1} ${date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}`;
  };

  // Determine if the message was sent by the current user
  const isSentByCurrentUser = sender === currentUserId;

  return (
    <div className={`message-wrapper ${isSentByCurrentUser ? 'my-message-wrapper' : 'other-message-wrapper'}`}>
      <div className={`message-container ${isSentByCurrentUser ? 'my-message' : 'other-message'}`}>
        {!isSentByCurrentUser && <span className="message-owner">{sender}</span>}
        <div className="message-content">{messageContent}</div>
      </div>
      <span className={`message-timestamp ${isSentByCurrentUser ? 'timestamp-right' : 'timestamp-left'}`}>
        {formatTimestamp(timestamp)}
      </span>
    </div>
  );
};

export default Message;
