import React, { useEffect, useState } from "react";
import settingsConfig from "./settingsconfig";
import "./settingspanel.css";

const SettingsPanel = ({ activeMode }) => {
  const config = settingsConfig[activeMode];
  const [isDarkMode, setIsDarkMode] = useState(
    JSON.parse(localStorage.getItem("darkMode")) || false
  );

  useEffect(() => {
    document.documentElement.setAttribute(
      "data-theme",
      isDarkMode ? "dark" : "light"
    );
  }, [isDarkMode]);

  const handleDarkModeToggle = () => {
    setIsDarkMode((prevState) => {
      const newState = !prevState;
      localStorage.setItem("darkMode", JSON.stringify(newState));
      return newState;
    });
  };

  if (!config) {
    return <div>Select a setting to configure.</div>;
  }

  return (
    <div className="settings-panel">
      <h3 className={"option-title"}>{config.title}</h3>
      <div className="settings-options">
        {config.options.map((option, index) => {
          switch (option.type) {
            case "checkbox":
              return (
                <div key={index} className="settings-option">
                  <span className="option-label">{option.label}</span>
                  {option.label === "Dark Mode" ? (
                    <div className="slider-container">
                      <input
                        type="checkbox"
                        id={`switch-${index}`}
                        checked={isDarkMode}
                        onChange={handleDarkModeToggle}
                      />
                      <label htmlFor={`switch-${index}`}></label>
                    </div>
                  ) : (
                    <div className="slider-container">
                      <input
                        type="checkbox"
                        id={`switch-${index}`}
                        defaultChecked={option.defaultValue}
                      />
                      <label htmlFor={`switch-${index}`}></label>
                    </div>
                  )}
                </div>
              );
            // Handle other option types...
            default:
              return null;
          }
        })}
      </div>
    </div>
  );
};

export default SettingsPanel;
