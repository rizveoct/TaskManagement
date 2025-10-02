module.exports = {
  content: ['./src/**/*.{html,ts}'],
  theme: {
    extend: {
      colors: {
        surface: '#0f172a',
        foreground: '#f1f5f9',
        accent: '#38bdf8',
        border: 'rgba(148, 163, 184, 0.2)',
        layer: {
          0: '#020617',
          1: '#111827',
          2: '#1e293b',
          3: '#334155',
        },
      },
    },
  },
  plugins: [],
};
