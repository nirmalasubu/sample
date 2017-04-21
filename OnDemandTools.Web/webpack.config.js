var path = require('path');
var webpack = require('webpack');
var CaseSensitivePathsPlugin = require('case-sensitive-paths-webpack-plugin');


module.exports = {
    context: path.resolve(__dirname, './App'),
    entry: {
        odtportal: './Index.jsx'
    },
    output: {
        path: __dirname + '/wwwroot/js/',
        filename: 'react.[name].bundle.js'
    },
    module: {
        loaders: [{
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
                loader: "style-loader!css-loader"
            },
            {
                test: /\.png$/,
                loader: "url-loader?limit=100000"
            },
            {
                test: /\.jpg$/,
                loader: "file-loader"
            },
            {
                test: /\.(woff|woff2)(\?v=\d+\.\d+\.\d+)?$/,
                loader: 'url-loader?limit=10000&mimetype=application/font-woff'
            },
            {
                test: /\.ttf(\?v=\d+\.\d+\.\d+)?$/,
                loader: 'url-loader?limit=10000&mimetype=application/octet-stream'
            },
            {
                test: /\.eot(\?v=\d+\.\d+\.\d+)?$/,
                loader: 'file-loader'
            },
            {
                test: /\.svg(\?v=\d+\.\d+\.\d+)?$/,
                loader: 'url-loader?limit=10000&mimetype=image/svg+xml'
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
    plugins: [
        new CaseSensitivePathsPlugin()
    ],
};