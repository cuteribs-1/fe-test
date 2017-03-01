var scale = 3;
var mspf = 1000 / 60;
var tileSize = {
	w: 16,
	h: 16
};
var cameraSize = {
	w: tileSize.w * 16,
	h: tileSize.h * 15
};
var spriteOffset = {
	x: 8,
	y: 8
};
var game, map, mapLayer, keys, cursor, player1;

var createGame = function () {
	game = new Phaser.Game(cameraSize.w * scale, cameraSize.h * scale,
		Phaser.AUTO,
		'', {
			preload: preload,
			create: create,
			update: update,
			render: render
		},
		true,
		false
	);
};

var preload = function () {
	game.load.spritesheet('sys01', 'assets/img/sys01.png', 21, 21);
	game.load.tilemap('mapData01', 'assets/img/map01-tileset.csv', null, Phaser.Tilemap.CSV)
	game.load.image('mapTiles01', 'assets/img/map01-tileset.png');
	game.load.spritesheet('actor', 'assets/img/stands.png', 16, 24);
};

var create = function () {
	//game.time.advancedTiming = true;

	game.world.setBounds(0, 0, tileSize.w * 23 * scale, tileSize.h * 21 * scale);

	map = game.add.tilemap('mapData01', 8, 8, 46, 42);
	map.addTilesetImage('map01', 'mapTiles01');

	mapLayer = map.createLayer(0, cameraSize.w, cameraSize.h);
	mapLayer.fixedToCamera = true;
	mapLayer.scale.set(scale);
	mapLayer.resizeWorld();

	player1 = game.add.sprite(tileSize.w * 3 * scale, (tileSize.h * 17 - 4) * scale, 'actor');
	player1.scale.setTo(scale);
	player1.animations.add('1', [9, 10, 11, 10], 4, true);

	cursor = game.add.sprite((tileSize.w * 3 + spriteOffset.x) * scale, (tileSize.h * 17 + spriteOffset.y) * scale, 'sys01');
	cursor.anchor.setTo(0.5, 0.5);
	cursor.scale.setTo(scale);
	cursor.animations.add('cursor', [0, 1], 4, true);

	game.camera.y = tileSize.h * 15 * scale;
	keys = game.input.keyboard.addKeys({
		up: Phaser.KeyCode.W,
		down: Phaser.KeyCode.S,
		left: Phaser.KeyCode.A,
		right: Phaser.KeyCode.D,
		a: Phaser.KeyCode.K,
		b: Phaser.KeyCode.J,
		start: Phaser.KeyCode.SPACEBAR,
	});
};

var update = function () {
	updateCamera();
	updateCursor();

	player1.animations.play('1');

	updateDebugInfo();
}

var render = function () {
	var mouse = game.input.activePointer;
	game.debug.text('mouse x: ' + mouse.x + ' | ' + mouse.worldX, 10, 00);
	game.debug.text('mouse y: ' + mouse.y + ' | ' + mouse.worldY, 10, 20);
	game.debug.text('camera x: ' + game.camera.x, 10, 40);
	game.debug.text('camera y: ' + game.camera.y, 10, 60);
	game.debug.text(game.time.fps || '--', 10, 80, "#00ff00");
};

var updateCursor = function () {
	// mouse control
	var mouse = game.input.activePointer;

	var cursorArea = {
		x: {
			min: tileSize.w * scale,
			max: cameraSize.w * scale - tileSize.w * scale - 1
		},
		y: {
			min: tileSize.h * scale,
			max: cameraSize.h * scale - tileSize.h * scale - 1
		}
	}

	if (mouse.x >= cursorArea.x.min && mouse.x <= cursorArea.x.max) {
		cursor.x = game.camera.x + Math.floor(mouse.x / tileSize.w / scale) * tileSize.w * scale + spriteOffset.x * scale;
	}

	if (mouse.y >= cursorArea.y.min && mouse.y <= cursorArea.y.max) {
		cursor.y = game.camera.y + Math.floor(mouse.y / tileSize.h / scale) * tileSize.h * scale + spriteOffset.y * scale;
	}

	// keyboard control
	var cursorPos = {
		x: cursor.x - spriteOffset.x * scale + game.camera.x,
		y: cursor.y - spriteOffset.y * scale + game.camera.y
	};

	var speed = 4 * scale;

	if (keys.up.isDown && cursorPos.y > tileSize * scale) {
		cursor.y = game.camera.y + cursorPos.y - tileSize.h * scale;
		//game.camera.y -= speed;
	} else if (keys.down.isDown && cursorPos.y < cameraSize.h * scale - tileSize.h * scale - 1) {
		cursor.y = game.camera.y + cursorPos.y + tileSize.h * scale;
		//game.camera.y += speed;
	}

	if (keys.left.isDown && cursorPos.x > tileSize * scale) {
		cursor.x = game.camera.x + cursorPos.x + tileSize.w * scale;
		//game.camera.x -= speed;
	} else if (keys.right.isDown && cameraSize.w * scale - tileSize.w * scale) {
		cursor.x = game.camera.x + cursorPos.x - tileSize.w * scale;
		//game.camera.x += speed;
	}

	cursor.animations.play('cursor');
};

var updateCamera = function () {

};

var updateDebugInfo = function () {};