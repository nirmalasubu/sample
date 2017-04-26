var path = require('path');
var webpack = require('webpack');
var CaseSensitivePathsPlugin = require('case-sensitive-paths-webpack-plugin');
var HtmlWebpackPlugin = require('html-webpack-plugin');


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
    plugins: [
        new CaseSensitivePathsPlugin(),
        new HtmlWebpackPlugin({
            hash: true
        })
    ],
};