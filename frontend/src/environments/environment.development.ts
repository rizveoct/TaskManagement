export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001',
  signalR: {
    taskHub: '/hubs/tasks',
    boardHub: '/hubs/boards',
    notificationHub: '/hubs/notifications',
    chatHub: '/hubs/chat',
    presenceHub: '/hubs/presence'
  }
};
