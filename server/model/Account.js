const mongoose = require('mongoose');
const { Schema } = mongoose;

const accountSchema = new Schema({
    username: String,
    password: String,
    record: Number,
    salt: String,
    lastAuth: Date,
    kills: Number,
    medals: [Number],
    items: [Number],
    isAdmin: Boolean,
})

mongoose.model('accounts', accountSchema);