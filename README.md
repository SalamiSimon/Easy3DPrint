# Easy3DPrint for SolidWorks

Easy3DPrint is a SolidWorks add-in designed to make it easy to open your 3D models directly in any slicing software. This add-in supports a range of file formats and is compatible with multiple slicing platforms.

You can also directly open multiple parts by creating an assembly.

<img width="750" alt="image" src="https://github.com/SalamiSimon/Easy3DPrint/assets/9504348/cd22afc3-f0ab-463e-bbd6-54b1f4f0ec9f">




## Supported File Formats

- STL (.stl)
- STEP (.step)
- 3D Manufacturing (.3mf)
- Additive Manufacturing (.amf)
- Solidworks Part (.sldprt)
- Polygon (.ply)

## Supported Slicing Software

- Ultimaker Cura
- Bambu Lab Studio
- eufymake Studio
- PrusaSlicer
- Slic3r
- Orca Slicer

## Installation

Run installer or build from source

## Build from source
- Open Easy3DPrint.sln in Visual Studio.
- In Solution Explorer, right-click the solution and select Properties.
- Go to Configuration Properties > Debug.
- Set Start External Program to your SolidWorks executable.
- Apply and save changes.
- Build & Run
- Select Build > Build Solution (Ctrl + Shift + B).
- Click Start Debugging (F5).

## Usage

After installation, you can access Easy3DPrint from the SolidWorks Tools menu:

1. Go to `Tools` > `Easy3DPrint`.
2. Choose the slicing software you wish to use from the Easy3DPrint menu.
3. If it's your first time using the add-in, you'll be prompted to enter the executable paths for your slicing software and select your preferred file format.
4. Once configured, select a part in SolidWorks and choose your desired slicing software from the Easy3DPrint menu to automatically export and open the part in the selected slicer.

## Configuration

To configure or change settings for Easy3DPrint:

1. Access the Easy3DPrint menu from `Tools` > `Easy3DPrint`.
2. Select `Settings` to open the configuration dialog.
3. Here, you can set the paths to your slicing software executables and choose default file formats for each slicer.

## Common issues
- Add-in not appearing after installation
  
Build from source or install v1.0.4. Create ticket if this happens in a later realease.
- A specific slicer is not implemented
  
A temporary solution is to just enter the .exe path to your slicer in any other slicer in settings. Almost all slicers open files the same way.

## Contributing

We welcome contributions to Easy3DPrint! If you have suggestions for new features, improvements, or want to add support for additional slicing software, please feel free to open an issue or submit a pull request. I'm still learning to code, so all feedback, positive or negative, is appreciated!
