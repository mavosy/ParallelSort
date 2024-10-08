<a id="readme-top"></a>

<!-- PROJECT SHIELDS -->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]

<!-- PROJECT LOGO -->
<br />
<div align="center">
<!--  <a href="https://github.com/mavosy/ParallelSort">
    <img src="images/logo.png" alt="Logo" width="80" height="80">
  </a> -->

<h3 align="center">ParallelSort</h3>

  <p align="center">
    An application that generates and sorts an array of N integers, comparing different sorting algorithm performances.
    <br />
    <a href="https://github.com/mavosy/ParallelSort"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/mavosy/ParallelSort/issues/new?labels=bug&template=bug-report---.md">Report Bug</a>
    ·
    <a href="https://github.com/mavosy/ParallelSort/issues/new?labels=enhancement&template=feature-request---.md">Request Feature</a>
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

<!-- [![Product Name Screen Shot][product-screenshot]](https://example.com) -->

**ParallelSort** is a C# and WPF application developed as a school project that implements amongst other things a parallelized merge sort algorithm. 
The project aims to demonstrate how to improve sorting efficiency by leveraging parallel computing. The algorithm is designed to divide 
the data into smaller chunks that can be sorted concurrently using multi-threading techniques. This project was developed collaboratively 
with a classmate to explore the challenges and advantages of parallel algorithms in comparison to traditional sorting techniques.

The project also includes additional sorting algorithms such as selection sort, standard sort, and variants of Top-N sorting algorithms for comparison, 
some of which are implemented by the teachers as comparing. The UI and the measuring code is not mine, just the merge sort.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Built With

* [![C#][csharp-shield]][csharp-url]
* [![WPF][wpf-shield]][wpf-url]
* MVVM Pattern

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

Follow these steps to get a local copy up and running.

### Prerequisites

- .NET 6.0 SDK or later
- Visual Studio 2022 or another IDE with WPF support

### Installation

1. Clone the repo.

2. Open the solution in Visual Studio and build the project.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- USAGE EXAMPLES -->
## Usage

1. Launch the application.
2. Generate an unsorted array of numbers
3. Compare results

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- ROADMAP -->
## Roadmap

- [ ]  Optimize the parallel merge sort by tuning thread allocation and load balancing.
- [ ]  Implement graphs or other statistical visualizations to compare performance across the different algorithms.

See the [open issues](https://github.com/mavosy/ParallelSort/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Top contributors:

<a href="https://github.com/mavosy/ParallelSort/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=mavosy/ParallelSort" alt="contrib.rocks image" />
</a>



<!-- LICENSE -->
## License

Distributed under the Unlicence License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

mavosy - maltesydow@gmail.com

Project Link: [https://github.com/mavosy/ParallelSort](https://github.com/mavosy/ParallelSort)

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/mavosy/ParallelSort.svg?style=for-the-badge
[contributors-url]: https://github.com/mavosy/ParallelSort/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/mavosy/ParallelSort.svg?style=for-the-badge
[forks-url]: https://github.com/mavosy/ParallelSort/network/members
[stars-shield]: https://img.shields.io/github/stars/mavosy/ParallelSort.svg?style=for-the-badge
[stars-url]: https://github.com/mavosy/ParallelSort/stargazers
[issues-shield]: https://img.shields.io/github/issues/mavosy/ParallelSort.svg?style=for-the-badge
[issues-url]: https://github.com/mavosy/ParallelSort/issues
[license-shield]: https://img.shields.io/github/license/mavosy/ParallelSort.svg?style=for-the-badge
[license-url]: https://github.com/mavosy/ParallelSort/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/malte-von-sydow
[product-screenshot]: images/screenshot.png
[csharp-shield]: https://custom-icon-badges.demolab.com/badge/C%23-%23239120.svg?logo=cshrp&logoColor=white
[csharp-url]: https://learn.microsoft.com/en-us/dotnet/csharp/
[wpf-shield]: https://img.shields.io/badge/WPF-512BD4?style=for-the-badge&logo=windows&logoColor=white
[wpf-url]: https://learn.microsoft.com/en-us/dotnet/desktop/wpf/
