const path = require('path');
const webpack = require('webpack');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');
const MomentLocalesPlugin = require('moment-locales-webpack-plugin');

    module.exports = (env = {}, argv = {}) => {
        const config = {
            mode: argv.mode || 'development', // we default to development when no 'mode' arg is passed
            entry: {
                general: './src/general.js'
            }, 
            output: {
                filename: '[name].js',
                path: path.resolve(__dirname, 'build'),
                publicPath: "/build/",
                hashFunction: "sha256"
            },
            plugins: [
                new MiniCssExtractPlugin({
                    filename:`[name].css`
                }),
                new webpack.ProvidePlugin({
                    $: 'jquery',
                    jquery: 'jquery',
                    'window.jquery': 'jquery',
                    Popper: ['popper.js', 'default']
                }),
                new UglifyJsPlugin(),
                new MomentLocalesPlugin({
                    localesToKeep: ['en'],
                })
            ],
            module: {
                rules: [
                    {
                        test: /\.css?$/,
                        use: [
                            {
                                loader: MiniCssExtractPlugin.loader,
                            },
                            "css-loader"
                        ]
                    },
                    {
                        test: /\.js?$/,
                        use: {
                            loader: 'babel-loader',
                            options: {
                                presets: [
                                    '@babel/preset-react',
                                    '@babel/preset-env']
                            }
                        }
                    },
                ]
            },
            optimization: {
                splitChunks: {
                    cacheGroups: {
                    vendor: {
                        test: /node_modules/,
                        name: 'vendor',
                        chunks: 'all',
                        }
                    }
                }
            }
        }
        return config;
};