# ASTRA Mobile App

ASTRA mobile app is a Xamarin Forms(Cross Platform) app build for supporting Android and IOS devices.

## Getting Started

### Introduction - ASTRA App

  Asset Tracking(ASTRA) Applications are available for Android and IOS devices. This app have features as follows

    * Configuration : App asks user to configure your REST api server Url.This feature makes this app generic for every tenant.

    * Login: User can Sign Up/ Sign In using this option. Azure Active directory B2C implementations required for login configuration.

    * Dispatch Shipment : This helps user to link sensor with an asset during the dispatch of shipment.

    * Receive Shipment: This helps user to check status (good or bad) of the asset. This will depends on the rules which user has set onto this asset's group.If any rule break's status would be bad.

    * Rules : User can apply several rules on asset based on the sensors linked to it via Web app.After checking status in receive shipment mode, user can also have list of rules break by the asset.


### Prerequisites

    * Asset Tracking Web App and REST Server deployement 
    * REST API Server URL for configuring mobile apps.
    * Supports Android OS 4+.
    * Supports IOS 8+.
    * Allows Photo Capture Capability to app.
    * Valid Sensor key(in QR code format)
    * Valid Asset key (in Barcode format)



## Installing

    * Download the app from shared url and copy it in your device.
    * Go to local path where app file is stored.
    * Click on app to start installation.
    * Once installation is complete, "ASTRA" app will appear on your menu.
    * Click on ASTRA to start app. When app is started for first time, it will ask for "REST API Url" on config screen.
    * Enter REST API URL and submit.
    * It will connect to REST Server and now app is ready to use.
    
## Built With

    * [Visual Studio 2017](https://www.visualstudio.com/downloads/) - The IDE
    * [Xamarin Studio for Mac](https://developer.xamarin.com/releases/studio/) - The IDE

