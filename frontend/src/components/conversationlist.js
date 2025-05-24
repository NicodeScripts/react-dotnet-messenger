import React from 'react';
import Contact from './contact'; // Ensure this component displays avatars
import './conversationlist.css'; // Styles for the container

const ConversationList = ({ conversations, onConversationClick, selectedContact }) => {
  return (
    <div className="conversation-list">
      {conversations.map(convo => (
        <Contact
          key={convo.id}
          name={convo.title} // Adjusted to use the title or name
          preview={convo.preview || 'No messages yet'} // Assuming preview might be in convo, if not replace with something else
          avatar={convo.avatar || 'path/to/default/avatar.png'} // Adjust as needed
          isSelected={selectedContact === convo.id}
          onClick={() => onConversationClick(convo)}
        />
      ))}
    </div>
  );
};

export default ConversationList;
