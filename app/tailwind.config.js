/** @type {import('tailwindcss').Config} */
export default {
  content: ['./index.html', './src/**/*.{js,jsx}'],
  theme: {
    extend: {
      colors: {
        // Paleta institucional CEFOPE
        cef: {
          bg:      '#0a1830',
          surface: '#0f2044',
          primary: '#185FA5',
          accent:  '#378ADD',
          text:    '#B5D4F4',
          muted:   '#85B7EB',
          success: '#1D9E75',
          border:  'rgba(56,122,221,0.2)',
        },
      },
    },
  },
  plugins: [],
}
