# IT Elective 2 Portfolio

**Rivel, John Cristian I. — BSIT 31E3**

An ASP.NET Core MVC portfolio of my IT Elective 2 projects and examinations. Every project has a thumbnail, a short description, a link to its GitHub repository, a detail page and its own comment section.

## Login

The site is behind a hardcoded login.

| Username | `admin` |
| --- | --- |
| Password | `admin123` |

The login is checked in `Services/HardcodedAuthenticator.cs`. The values can be overridden through the `PortfolioLogin` section of `appsettings.json`.

## Features

- **Table of contents** — projects grouped by term (Prelim, Midterm, Prefinals, Finals) with search and comment counts.
- **Detail page per project** — thumbnail, tags, description, GitHub link, previous/next navigation.
- **Comments per project** — post a comment, and delete the ones you posted. Comments are kept in memory, so they reset when the app restarts.
- **Hardcoded login** — cookie authentication, with a lockout after 5 failed attempts in 5 minutes.

## Repositories

| # | Project | Term | Repository |
| --- | --- | --- | --- |
| 1 | Prelim Quiz 1 | Prelim | [BSIT_31E3_PRELIM_Q1_Rivel_JohnCristian](https://github.com/JohnRivel/BSIT_31E3_PRELIM_Q1_Rivel_JohnCristian) |
| 2 | Prelim Activity 1 | Prelim | [BSIT31A3_Prelim_A1_RivelJohnCristian](https://github.com/JohnRivel/BSIT31A3_Prelim_A1_RivelJohnCristian) |
| 3 | Prelim Hands-On 1 | Prelim | [BSIT31E3_PRELIM_H1_Rivel_JohnCristian](https://github.com/JohnRivel/BSIT31E3_PRELIM_H1_Rivel_JohnCristian) |
| 4 | Prelim Hands-On 1 (Alternate) | Prelim | [BSIT31E3_PRELIM_H1_Rivel_JohnCristianI.](https://github.com/JohnRivel/BSIT31E3_PRELIM_H1_Rivel_JohnCristianI.) |
| 5 | Prelim Hands-On 2 | Prelim | [BSIT31E3_PRELIM_H2_Rivel_JohnCristian](https://github.com/JohnRivel/BSIT31E3_PRELIM_H2_Rivel_JohnCristian) |
| 6 | Prelim Exam | Prelim | [IT_ELECTIVE_2_PRELIM_EXAM_Rivel_JohnCristian](https://github.com/JohnRivel/IT_ELECTIVE_2_PRELIM_EXAM_Rivel_JohnCristian) |
| 7 | Midterm Hands-On 1 to 3 | Midterm | [RIVEL_IT_ELECTIVE_2_MIDTERM_H1_H2_H3](https://github.com/JohnRivel/RIVEL_IT_ELECTIVE_2_MIDTERM_H1_H2_H3) |
| 8 | Midterm Quiz 2 Backup | Midterm | [IT_ELECTIVE_2_MIDTERM_Q2_Rivel_JohnCristian_Backup](https://github.com/JohnRivel/IT_ELECTIVE_2_MIDTERM_Q2_Rivel_JohnCristian_Backup) |
| 9 | Midterm Quiz 3 | Midterm | [Rivel_IT_ELECTIVE_2_MIDTERM_Q3](https://github.com/JohnRivel/Rivel_IT_ELECTIVE_2_MIDTERM_Q3) |
| 10 | Midterm Exam | Midterm | [RIVEL_IT_ELECTIVE_2_MIDTERM_EXAM](https://github.com/JohnRivel/RIVEL_IT_ELECTIVE_2_MIDTERM_EXAM) |
| 11 | Midterm Exam Set 5 | Midterm | [IT_ELECTIVE_2_MIDTERM_EXAM_SET_5_RIVEL_JOHNCRISTIAN](https://github.com/JohnRivel/IT_ELECTIVE_2_MIDTERM_EXAM_SET_5_RIVEL_JOHNCRISTIAN) |
| 12 | Prefinal Project | Prefinals | [ITELECTIVE2_PREFINAL_RIVEL](https://github.com/JohnRivel/ITELECTIVE2_PREFINAL_RIVEL) |
| 13 | Prefinal Exam | Prefinals | [IT_ELECTIVE_2_BSIT31E3_PREFINAL_EXAM_RIVEL_JOHNCRISTIAN](https://github.com/JohnRivel/IT_ELECTIVE_2_BSIT31E3_PREFINAL_EXAM_RIVEL_JOHNCRISTIAN) |

## Run

Requires the .NET 8 SDK.

```bash
dotnet run
```

Then open the URL printed in the terminal and sign in with the credentials above. In Visual Studio, open `Portfolio.slnx`, set `Portfolio` as the startup project and press F5.

