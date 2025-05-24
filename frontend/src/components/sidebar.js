import React from 'react';
import './sidebar.css';
import { IoMdMenu, IoIosChatboxes, IoMdSettings, IoIosCall } from 'react-icons/io';

const SideBar = ({ selectedMode, setSelectedMode, onToggleSettingsMenu }) => {
  return (
    <div className="sidebar-container">
      <div 
        className={`menu-button ${selectedMode === 'menu' ? 'selected' : ''}`}
        onClick={() => setSelectedMode('menu')}
      >
        <IoMdMenu className="icon" />
      </div>

      {/* Buttons container for chat and call */}
      <div className="buttons-container">
        <div 
          className={`sidebar-button ${selectedMode === 'chat' ? 'selected' : ''}`}
          onClick={() => setSelectedMode('chat')}
        >
          <IoIosChatboxes className="icon" />
        </div>
        <div 
          className={`sidebar-button ${selectedMode === 'call' ? 'selected' : ''}`}
          onClick={() => setSelectedMode('call')}
        >
          <IoIosCall className="icon" />
        </div>
      </div>

      {/* Profile widget */}
      <div className="profile-widget">
        <img 
          src="No-profile.png" 
          alt="User Avatar" 
          className="profile-image"
        />
      </div>

      {/* Settings button at the bottom */}
      <div 
        className="settings-button"
        onClick={onToggleSettingsMenu}
      >
        <IoMdSettings className="icon" />
      </div>
    </div>
  );
};

export default SideBar;
