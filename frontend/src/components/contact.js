import React, { useState, useEffect, useRef } from 'react';
import ReactDOM from 'react-dom';
import './contact.css';

const Contact = ({ name, preview, avatar, isSelected, onClick, small, showMenu, isChatDropdown, onAddUser, onSeeProfile }) => {
  const [isVisible, setIsVisible] = useState(false);  // Controls the animation visibility
  const [isExiting, setIsExiting] = useState(false);  // Controls the exit animation
  const contactRef = useRef(null);

  useEffect(() => {
    if (showMenu) {
      // Open the menu with a slight delay for smooth entry
      const timer = setTimeout(() => setIsVisible(true), 50); 
      return () => clearTimeout(timer);
    } else {
      // Trigger exit animation by setting isVisible to false
      setIsVisible(false);
      setIsExiting(true); // Start exit phase
      // Delay unmounting the menu for the duration of the animation (0.3s)
      const exitTimer = setTimeout(() => setIsExiting(false), 300);
      return () => clearTimeout(exitTimer);
    }
  }, [showMenu]);

  const renderMenu = () => {
    if (showMenu || isExiting) { // Keep the menu visible while exiting
      const rect = contactRef.current.getBoundingClientRect();
      return ReactDOM.createPortal(
        <div
          className={`contact-menu ${isVisible ? 'show' : 'hide'}`} // Use 'show' and 'hide' classes for entry/exit animation
          style={{
            top: rect.top + window.scrollY,
            left: rect.right + 10,
          }}
        >
          <button className="contact-menu-option" onClick={onAddUser}>Add User</button>
          <div className="contact-menu-header"></div> {/* Separator */}
          <button className="contact-menu-option" onClick={onSeeProfile}>See Profile</button>
        </div>,
        document.body
      );
    }
    return null;
  };

  return (
    <>
      <div 
        className={`contact-item ${isSelected ? 'selected' : ''} ${small ? 'small' : ''} ${isChatDropdown ? 'chat-dropdown-item' : ''}`} 
        onClick={onClick}
        ref={contactRef}
      >
        <div className="contact-avatar">
          <img 
            src={"No-profile.png"} 
            alt={`${name}'s avatar`} 
            className={`contact-avatar-img ${small ? 'small' : ''}`} 
          />
        </div>
        <div className="contact-info">
          <h3 className={`contact-name ${small ? 'small' : ''}`}>{name}</h3>
          <p className={`contact-preview-text ${small ? 'small' : ''}`}>{preview}</p>
        </div>
      </div>
      {renderMenu()}
    </>
  );
};

export default Contact;
