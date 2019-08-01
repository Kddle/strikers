import express from 'express';
import cors from 'cors';
import uuidv4 from 'uuid/v4';
import 'dotenv/config';

const app = express();

app.use(cors());

app.get('/', (req,res) => {
    res.send(`Welcome on the Strikers API ! You are visitor : ${uuidv4()}`);
});

// Login the player
app.post('/login', (req, res) => {
    res.send(`Not implemented !`);
});

// Register new player
app.post('/register', (req, res) => {
    res.send(`Not implemented !`);
});

// Get list of all opened rooms
app.get('/rooms', (req, res) => {
    res.send(`Not implemented !`);
});

// Get specific room infos
app.get('/rooms/:roomId', (req, res) => {
    res.send(`Not implemented !`);
});

// Create new room
app.post('/rooms', (req, res) => {
    res.send(`Not implemented !`);
});

app.listen(process.env.WEBPORT, () => {
    console.log('Strikers WebServer is listening on port ' + process.env.WEBPORT);
});
