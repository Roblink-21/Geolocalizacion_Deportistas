const express = require('express');
const app = express();
const http = require('http');
const server = http.createServer(app);
const { Server } = require("socket.io");
const io = new Server(server, { cors: { origin: '*' } });

let clients = [];

io.on('connection', (socket) => {
    console.log("Cliente conectado");
    socket.on("set_position", (data) => {
      let index = clients.map(item => item.id).indexOf(socket.id);
      if (index == -1) {
        clients.push({
          id: socket.id,
          location: data,
          ref: Date.now()
        })
      } else {
        clients[index].location = data
      }
      io.emit("clients_update", clients.map(item => ({
        location: item.location,
        ref: item.ref
      })))
    });
    socket.on("disconnect", () => {
      clients = clients.filter(item => item.id != socket.id)
    });
  });

server.listen(3000, () => {
  console.log('listening on *:3000');
});