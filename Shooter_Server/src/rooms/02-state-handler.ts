import { Room, Client } from "colyseus";
import { Schema, type, MapSchema } from "@colyseus/schema";

export class Vector3 {
    x = 0;
    y = 0;
    z = 0;

    constructor(x, y, z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

export class Player extends Schema {
    @type("uint8")
    loss = 0;

    @type("int8")
    maxHP = 0;
    
    @type("int8")
    currentHP = 0;

    @type("number")
    speed = 0;

    @type("number")
    pX = 0;

    @type("number")
    pY = 0;

    @type("number")
    pZ = 0;

    @type("number")
    vX = 0;

    @type("number")
    vY = 0;

    @type("number")
    vZ = 0;

    @type("number")
    rX = 0;

    @type("number")
    rY = 0;
}

export class State extends Schema {
    @type({ map: Player })
    players = new MapSchema<Player>();

    something = "This attribute won't be sent to the client-side";

    createPlayer(sessionId: string, data: any, pos: Vector3) {
        const player = new Player();
        player.speed = data.speed;
        player.maxHP = data.hp;
        player.currentHP = data.hp;

        player.pX = pos.x;
        player.pY = pos.y;
        player.pZ = pos.z;

        this.players.set(sessionId, player);
    }

    removePlayer(sessionId: string) {
        this.players.delete(sessionId);
    }

    movePlayer (sessionId: string, data: any) {
        const player = this.players.get(sessionId);
        player.pX = data.pX;
        player.pY = data.pY;
        player.pZ = data.pZ;
        player.vX = data.vX;
        player.vY = data.vY;
        player.vZ = data.vZ;
        player.rX = data.rX;
        player.rY = data.rY;
    }
}

export class StateHandlerRoom extends Room<State> {
    maxClients = 2;

    spawnPoints : Array<Vector3> = [new Vector3(18.8,1.67850351,1.6),
        new Vector3(-19.1,1.67850494,-16),
        new Vector3(-17.3,1.67850304,17)];

    onCreate (options) {
        console.log("StateHandlerRoom created!", options);

        this.setState(new State());

        this.onMessage("move", (client, data) => {
            //console.log("StateHandlerRoom received message from", client.sessionId, ":", data);
            this.state.movePlayer(client.sessionId, data);
        });

        this.onMessage("shoot", (client, data) => {
            this.broadcast("Shoot", data, {except: client});
        })

        this.onMessage("gun", (client, data) => {
            this.broadcast("Gun", data, {except: client});
        })

        this.onMessage("damage", (client, data) => {
            const clientId = data.id;
            const player = this.state.players.get(data.id);
            
            let hp = player.currentHP - data.value;

            if(hp > 0){
                player.currentHP = hp;
                return;
            }

            player.loss++;
            player.currentHP = player.maxHP;

            for(var i = 0; i < this.clients.length; i++){
                if(this.clients[i].sessionId != clientId) continue;

                let spawnPointID = Math.floor(Math.random() * this.spawnPoints.length);

                const newPos = this.spawnPoints[spawnPointID];

                const x = newPos.x;
                const y = newPos.y;
                const z = newPos.z;

                const message = JSON.stringify({x, y, z});
                this.clients[i].send("Restart", message);
            }
        })
    }

    onAuth(client, options, req) {
        return true;
    }

    onJoin (client: Client, data: any) {
        if(this.clients.length > 1) this.lock();
        client.send("hello", "world");
        
        let spawnPointID = Math.floor(Math.random() * this.spawnPoints.length);

        const newPos = this.spawnPoints[spawnPointID];

        this.state.createPlayer(client.sessionId, data, newPos);
    }

    onLeave (client) {
        this.state.removePlayer(client.sessionId);
    }

    onDispose () {
        console.log("Dispose StateHandlerRoom");
    }
}
