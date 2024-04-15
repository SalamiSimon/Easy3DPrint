# Easy3DPrint for SolidWorks

Easy3DPrint is a SolidWorks add-in designed to streamline the process of preparing your 3D models for printing. With just a few clicks, you can directly open your parts in various 3D printing slicing software, saving you time and enhancing your workflow. This add-in supports a range of file formats and is compatible with multiple slicing platforms.

## Supported File Formats

- STL (.stl)
- OBJ (.obj)
- STEP (.step)
- 3MF (.3mf)

## Supported Slicing Software

- Ultimaker Cura
- Bambu Lab
- AnkerMake
- PrusaSlicer
- Slic3r

## Installation

To install the Easy3DPrint add-in for SolidWorks, follow these steps:

1. **Download the Add-in**: Download the latest version of Easy3DPrint from the Releases section of this GitHub repository.
2. **Install the Add-in**: Copy the downloaded `.dll` file to your SolidWorks add-ins directory. This location can vary depending on your SolidWorks installation, but it is typically found under `C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\`.
3. **Enable the Add-in in SolidWorks**:
    - Open SolidWorks.
    - Go to `Tools` > `Add-Ins`.
    - In the Add-Ins window, find Easy3DPrint in the list of available add-ins.
    - Check the boxes under "Active Add-Ins" and "Start Up" to enable Easy3DPrint.
    - Click `OK` to apply the changes.

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

## Contributing

We welcome contributions to Easy3DPrint! If you have suggestions for new features, improvements, or want to add support for additional slicing software, please feel free to open an issue or submit a pull request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.