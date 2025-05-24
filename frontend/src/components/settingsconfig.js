// settingsConfig.js

const settingsConfig = {
    general: {
      title: 'General Settings',
      options: [
        { label: 'Enable Notifications', type: 'checkbox', defaultValue: true },
        { label: 'Dark Mode', type: 'checkbox', defaultValue: false },
      ],
    },
    chats: {
      title: 'Chat Settings',
      options: [
        { label: 'Save Chat History', type: 'checkbox', defaultValue: true },
        { label: 'Font Size', type: 'select', defaultValue: 'medium', options: ['small', 'medium', 'large'] },
      ],
    },
    personalization: {
      title: 'Personalization',
      options: [
        { label: 'Theme Color', type: 'select', defaultValue: 'blue', options: ['blue', 'red', 'green'] },
        { label: 'Profile Visibility', type: 'checkbox', defaultValue: false },
      ],
    },
    account: {
      title: 'Account Settings',
      options: [
        { label: 'Change Password', type: 'button', action: () => alert('Password change triggered!') },
        { label: 'Delete Account', type: 'button', action: () => alert('Account deletion triggered!') },
      ],
    },
  };
  
  export default settingsConfig;
  