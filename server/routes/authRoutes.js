const mongoose = require("mongoose");
const Account = mongoose.model('accounts');
const argon2i = require('argon2-ffi').argon2i;
const crypto = require('crypto');
    

const passwordRegex = new RegExp("(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{8,})");
 
module.exports = app => {

    app.get('/', (req, res) => {
        res.send("Bem vindo(a)!");
    })

    app.post('/account/login', async (req, res) => {

        const response = {};

        const { rUsername, rPassword } = req.body;
        // if ( rUsername === null || !passwordRegex.test(rPassword)){
        if ( rUsername === null || rPassword == null ){
            response.code = 1;
            response.msg = "Invalid credentials.";
            res.send(response);
            return;
        };
        // const userAccount = await Account.findOne( { username: rUsername }, 'username isAdmin password' );
        const userAccount = await Account.findOne( { username: rUsername } );
       
        if(userAccount !== null){
            argon2i.verify(userAccount.password, rPassword).then ( async success => {
                if(success){
                    userAccount.lastAuth = Date.now();
                    await userAccount.save();

                    response.code = 0;
                    response.msg = "Account found."
                    response.data = userAccount;
                    // response.data = ( ({username, kills, medals, items, record, isAdmin}) => ({username, kills, medals, items, record, isAdmin}) )(userAccount);
                    res.send(response);

                    return;
                }
                response.code = 1;
                response.msg = "Invalid credentials."
                res.send(response);
            })
            return;
        }
        
        response.code = 1;
        response.msg = "Invalid credentials."
        res.send(response);
    
    });

    app.post('/account/create', async (req, res) => {

        const response = {};

        const { rUsername, rPassword } = req.body;
        if ( rUsername === null || rPassword === null) {
            response.code = 1;
            response.msg = "Invalid credentials.";
            res.send(response);
            return;
        }
        // if(!passwordRegex.test(rPassword)){
        //     response.code = 3;
        //     response.msg = "Unsafe password.";
        //     res.send(response);
        //     return;
        // } 


        // const userAccount = await Account.findOne( { username: rUsername }, '_id' );
        const userAccount = await Account.findOne( { username: rUsername } );
        if (userAccount !== null) {
            response.code = 2;
            response.msg = "Username is already taken.";
            res.send(response);
            return;
        };

        crypto.randomBytes(32, (err, salt)=>{
            argon2i.hash(rPassword, salt).then(async hash => {
                const newAccount = new Account({
                    username: rUsername,
                    password: hash,
                    record: 0,
                    salt: salt,
                    lastAuth: Date.now(),
                    kills: 0,
                    medals: [],
                    items: [],
                    isAdmin: false,
                })
                await newAccount.save();

                response.code = 0;
                response.msg = "Account created.";
                response.data = response.data = newAccount;
                // response.data = response.data = ( ({username, kills, medals, items, record, isAdmin}) => ({username, kills, medals, items, record, isAdmin}) )(newAccount);;
                res.send(response);

            }).catch( err => res.send( err ) );
        })

    })
}
