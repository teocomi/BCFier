const { download } = require('google-fonts-helper');
const dir = './src/assets/fonts/';
const robotoDownloader = download('https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500&display=swap', {
  base64: false,
  overwriting: false,
  outputDir: dir + 'fonts',
  stylePath: 'fonts.css',
  fontsDir: '',
  fontsPath: './'
});

robotoDownloader.execute();

const materialIconsDownloader = download('https://fonts.googleapis.com/icon?family=Material+Icons', {
base64: false,
  overwriting: false,
  outputDir: dir + 'icons',
  stylePath: 'fonts.css',
  fontsDir: '',
  fontsPath: './'
});

materialIconsDownloader.execute();

console.log('Fonts copying is done!');
