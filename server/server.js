const express = require('express');
const app = express();
const bodyParser = require('body-parser');
const cors = require('cors');
require("dotenv").config();

app.use(bodyParser.urlencoded({extended:false}));
app.use(cors("*"));


// DATABASE
require("mongoose").connect(process.env.DB_URI, { useNewUrlPArser: true, useUnifiedTopology: true })

// DB MODELS
require('./model/Account'); // rodando o codigo só

// routes
require('./routes/authRoutes.js')(app);

const PORT = process.env.PORT || 8080;
app.listen(PORT, () => console.log( `Listening on ${PORT}` ) );