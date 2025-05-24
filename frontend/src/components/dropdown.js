import React, { useState } from 'react';
import './dropdown.css';

const Dropdown = ({ options, selected, setSelected, placeholder, Icon, title }) => {
  const [isOpen, setIsOpen] = useState(false);

  const handleOptionClick = (option) => {
    setSelected(option);
    setIsOpen(false);
  };

  return (
    <div className="dropdown-container">
      <div
        className="dropdown-button"
        onClick={() => setIsOpen(!isOpen)}
      >
        <span>{selected ? selected.label : placeholder}</span>
        {Icon && <Icon className="dropdown-icon"/>}
      </div>
      <div className={`dropdown-menu ${isOpen ? 'open' : ''}`}>
        {title && <div className="dropdown-title">{title}</div>}
        {options.map((option, index) => (
          <div
            key={index}
            className="dropdown-option"
            onClick={() => handleOptionClick(option)}
          >
            {option.label}
          </div>
        ))}
      </div>
    </div>
  );
};

export default Dropdown;
