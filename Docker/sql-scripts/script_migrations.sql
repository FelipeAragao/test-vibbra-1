CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Users` (
    `UserId` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Email` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Login` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
    `Password` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Users` PRIMARY KEY (`UserId`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Deals` (
    `DealId` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `Type` int NOT NULL,
    `Value` decimal(65,30) NOT NULL,
    `Description` varchar(150) CHARACTER SET utf8mb4 NULL,
    `TradeFor` varchar(100) CHARACTER SET utf8mb4 NULL,
    `UrgencyType` int NOT NULL,
    CONSTRAINT `PK_Deals` PRIMARY KEY (`DealId`),
    CONSTRAINT `FK_Deals_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Invites` (
    `InviteId` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `UserInvitedId` int NOT NULL,
    `Name` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `Email` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Invites` PRIMARY KEY (`InviteId`),
    CONSTRAINT `FK_Invites_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`) ON DELETE RESTRICT,
    CONSTRAINT `FK_Invites_Users_UserInvitedId` FOREIGN KEY (`UserInvitedId`) REFERENCES `Users` (`UserId`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `UserLocations` (
    `UserLocationId` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `Lat` double NOT NULL,
    `Lng` double NOT NULL,
    `Address` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `City` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `State` varchar(2) CHARACTER SET utf8mb4 NOT NULL,
    `ZipCode` int NOT NULL,
    `Active` tinyint(1) NOT NULL,
    CONSTRAINT `PK_UserLocations` PRIMARY KEY (`UserLocationId`),
    CONSTRAINT `FK_UserLocations_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Bids` (
    `BidId` int NOT NULL AUTO_INCREMENT,
    `UserId` int NOT NULL,
    `DealId` int NOT NULL,
    `Accepted` tinyint(1) NOT NULL,
    `Value` decimal(65,30) NOT NULL,
    `Description` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Bids` PRIMARY KEY (`BidId`),
    CONSTRAINT `FK_Bids_Deals_DealId` FOREIGN KEY (`DealId`) REFERENCES `Deals` (`DealId`) ON DELETE CASCADE,
    CONSTRAINT `FK_Bids_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `DealImages` (
    `DealImageId` int NOT NULL AUTO_INCREMENT,
    `DealId` int NOT NULL,
    `ImageUrl` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_DealImages` PRIMARY KEY (`DealImageId`),
    CONSTRAINT `FK_DealImages_Deals_DealId` FOREIGN KEY (`DealId`) REFERENCES `Deals` (`DealId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `DealLocation` (
    `DealLocationId` int NOT NULL AUTO_INCREMENT,
    `DealId` int NOT NULL,
    `Lat` double NOT NULL,
    `Lng` double NOT NULL,
    `Address` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `City` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `State` varchar(2) CHARACTER SET utf8mb4 NOT NULL,
    `ZipCode` int NOT NULL,
    CONSTRAINT `PK_DealLocation` PRIMARY KEY (`DealLocationId`),
    CONSTRAINT `FK_DealLocation_Deals_DealId` FOREIGN KEY (`DealId`) REFERENCES `Deals` (`DealId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Deliveries` (
    `DeliveryId` int NOT NULL AUTO_INCREMENT,
    `DealId` int NOT NULL,
    `UserId` int NOT NULL,
    `DeliveryPrice` decimal(65,30) NOT NULL,
    CONSTRAINT `PK_Deliveries` PRIMARY KEY (`DeliveryId`),
    CONSTRAINT `FK_Deliveries_Deals_DealId` FOREIGN KEY (`DealId`) REFERENCES `Deals` (`DealId`) ON DELETE CASCADE,
    CONSTRAINT `FK_Deliveries_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Messages` (
    `MessageId` int NOT NULL AUTO_INCREMENT,
    `DealId` int NOT NULL,
    `UserId` int NOT NULL,
    `Title` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `TextMessage` varchar(1000) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Messages` PRIMARY KEY (`MessageId`),
    CONSTRAINT `FK_Messages_Deals_DealId` FOREIGN KEY (`DealId`) REFERENCES `Deals` (`DealId`) ON DELETE CASCADE,
    CONSTRAINT `FK_Messages_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `DeliverySteps` (
    `DeliveryStepsId` int NOT NULL AUTO_INCREMENT,
    `DeliveryId` int NOT NULL,
    `Date` datetime(6) NOT NULL,
    `DeliveryStatus` int NOT NULL,
    `Active` tinyint(1) NOT NULL,
    CONSTRAINT `PK_DeliverySteps` PRIMARY KEY (`DeliveryStepsId`),
    CONSTRAINT `FK_DeliverySteps_Deliveries_DeliveryId` FOREIGN KEY (`DeliveryId`) REFERENCES `Deliveries` (`DeliveryId`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

INSERT INTO `Users` (`UserId`, `Email`, `Login`, `Name`, `Password`)
VALUES (1, 'super@user.com', 'super', 'Super User', 'sQYVruLpNUD+dTbZGoDQc4QWpPyuwcA3mv7nYpp6eCA=:4ZRWQMD+f+980pqysc9s+g==');

INSERT INTO `UserLocations` (`UserLocationId`, `Active`, `Address`, `City`, `Lat`, `Lng`, `State`, `UserId`, `ZipCode`)
VALUES (1, FALSE, '123 Main St', 'São Paulo', -23.550519999999999, -46.633308, 'SP', 1, 12345);

CREATE INDEX `IX_Bids_DealId` ON `Bids` (`DealId`);

CREATE INDEX `IX_Bids_UserId` ON `Bids` (`UserId`);

CREATE INDEX `IX_DealImages_DealId` ON `DealImages` (`DealId`);

CREATE UNIQUE INDEX `IX_DealLocation_DealId` ON `DealLocation` (`DealId`);

CREATE INDEX `IX_Deals_UserId` ON `Deals` (`UserId`);

CREATE UNIQUE INDEX `IX_Deliveries_DealId` ON `Deliveries` (`DealId`);

CREATE INDEX `IX_Deliveries_UserId` ON `Deliveries` (`UserId`);

CREATE INDEX `IX_DeliverySteps_DeliveryId` ON `DeliverySteps` (`DeliveryId`);

CREATE INDEX `IX_Invites_UserId` ON `Invites` (`UserId`);

CREATE INDEX `IX_Invites_UserInvitedId` ON `Invites` (`UserInvitedId`);

CREATE INDEX `IX_Messages_DealId` ON `Messages` (`DealId`);

CREATE INDEX `IX_Messages_UserId` ON `Messages` (`UserId`);

CREATE INDEX `IX_UserLocations_UserId` ON `UserLocations` (`UserId`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20240924205807_CompleteDev', '8.0.8');

COMMIT;