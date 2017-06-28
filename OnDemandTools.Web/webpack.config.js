var path = require('path');
var webpack = require('webpack');
var CaseSensitivePathsPlugin = require('case-sensitive-paths-webpack-plugin');


var isProd = (process.env.NODE_ENV === 'production');

// Conditionally return a list of plugins to use based on the current environment.
// Repeat this pattern for any other config key (ie: loaders, etc).
function getPlugins() {
  var plugins = [];

  // Always expose NODE_ENV to webpack, you can now use `process.env.NODE_ENV`
  // inside your code for any environment checks
  plugins.push(new webpack.DefinePlugin({
    'process.env': {
      'NODE_ENV': JSON.stringify(process.env.NODE_ENV)
    }
  }));

  // Add common plugins
  plugins.push(new CaseSensitivePathsPlugin());

  // Conditionally add plugins for Production builds.
  if (isProd) {
    plugins.push(new webpack.optimize.UglifyJsPlugin({
            minimize: true,
            compress: {
                warnings: false
            }
        }));

    plugins.push(new webpack.optimize.AggressiveMergingPlugin());
  }

  // Conditionally add plugins for Development
  else {
    
  }

  return plugins;
}

module.exports = {
    context: path.resolve(__dirname, './App'),
    entry: {
        odtportal: './Index.jsx'
    },
    output: {
        path: __dirname + '/wwwroot',
        filename: './react.[name].bundle.js'
    },
    module: {
        loaders: [
            {
                test: /.jsx?$/,
                loader: "babel-loader",
                exclude: /node_modules/,
                query: {
                    presets: ['es2015', 'react'],
                    plugins: ['react-html-attrs', 'transform-class-properties', 'transform-decorators-legacy']
                }
            },
            {
                test: /\.css$/,
                loader: "style-loader!css-loader?&name=./js/wbassets/[hash].[ext]"
            },
            {
                test: /\.png$/,
                loader: "url-loader?limit=100000&name=./js/wbassets/[hash].[ext]"
            },
            {
                test: /\.jpg$/,
                loader: "file-loader?&name=./js/wbassets/[hash].[ext]"
            },
            {
                test: /\.gif$/,
                loader: "url-loader?limit=100000&name=./js/wbassets/[hash].[ext]"
            },
            {
                test: /\.(woff|woff2)(\?v=\d+\.\d+\.\d+)?$/,
                loader: 'url-loader?limit=10000&mimetype=application/font-woff&name=./js/wbassets/[hash].[ext]'
            },
            {
                test: /\.ttf(\?v=\d+\.\d+\.\d+)?$/,
                loader: 'url-loader?limit=10000&mimetype=application/octet-stream&name=./js/wbassets/[hash].[ext]'
            },
            {
                test: /\.eot(\?v=\d+\.\d+\.\d+)?$/,
                loader: 'file-loader?&name=./js/wbassets/[hash].[ext]'
            },
            {
                test: /\.svg(\?v=\d+\.\d+\.\d+)?$/,
                loader: 'url-loader?limit=10000&mimetype=image/svg+xml&name=./js/wbassets/[hash].[ext]'
            }
        ]
    },
    resolve: {
        extensions: [".js", ".jsx", ".es6"],
        modules: [
            path.resolve('./App'),
            path.resolve('./node_modules')
        ]

    },
    plugins: getPlugins(),
};