"use strict";

var gulp = require("gulp"),
    path = require('path'),
    copyResources = require('./node_modules/@abp/aspnetcore.mvc.ui/gulp/copy-resources.js');

exports.default = function(done){
    copyResources(path.resolve('./'));
    done();
};