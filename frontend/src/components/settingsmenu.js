import { useState, useEffect, useRef } from "react";
import "./settingsmenu.css";
import SettingsPanel from "./settingspanel";

const SettingsMenu = ({ isOpen, onClose }) => {
  const settingModes = [
    { label: "General", value: "general" },
    { label: "Chats", value: "chats" },
    { label: "Personalization", value: "personalization" },
    { label: "Account", value: "account" },
  ];

  const [activeMode, setActiveMode] = useState(null);
  const menuRef = useRef(null);

  useEffect(() => {
    const savedTheme = localStorage.getItem("theme") || "light";
    document.documentElement.setAttribute("data-theme", savedTheme);
  }, []);

  const handleModeClick = (mode) => {
    setActiveMode(mode);
  };

  const handleClickOutside = (event) => {
    if (menuRef.current && !menuRef.current.contains(event.target)) {
      onClose();
      setActiveMode(null);
    }
  };

  useEffect(() => {
    if (isOpen) {
      document.addEventListener("mousedown", handleClickOutside);
    }

    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [isOpen]);

  return (
    <div
      ref={menuRef}
      className={`settingsmenu-container ${isOpen ? "open" : ""}`}
    >
      <div className="settingsmenu-options">
        {settingModes.map((mode) => (
          <button
            key={mode.value}
            className="settingsmenu-option-button"
            onClick={() => handleModeClick(mode.value)}
          >
            {mode.label}
          </button>
        ))}
      </div>
      <div className="settingsmenu-panel">
        {activeMode ? (
          <SettingsPanel activeMode={activeMode} />
        ) : (
          <div className="settingsmenu-placeholder">
            <p>Select a setting to configure.</p>
          </div>
        )}
      </div>
    </div>
  );
};

export default SettingsMenu;
