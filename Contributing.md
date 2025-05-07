
# Contributing to SeroGlint.DotNet

We appreciate your interest in contributing to **SeroGlint.DotNet**! Whether you're fixing a bug, adding a new feature, or improving documentation, your contributions help make this library better for everyone.

---

## 🛠️ **How to Contribute**

### 1️⃣ **Fork & Clone the Repository**

1. Fork this repository on GitHub.
2. Clone your fork locally:

   ```bash
   git clone https://github.com/YOUR-USERNAME/SeroGlint.DotNet.git
   cd SeroGlint.DotNet
   ```

3. Add the upstream repository to keep your fork updated:

   ```bash
   git remote add upstream https://github.com/Iterix-US/SeroGlint.DotNet.git
   ```

### 2️⃣ **Create a Feature Branch**

Before making changes, create a new branch:

```bash
git checkout -b feature/my-new-feature
```

### 3️⃣ **Make Your Changes**

- Ensure your code follows .NET best practices.
- Write clear and concise code with proper documentation.
- Run tests to verify your changes:

  ```bash
  dotnet test
  ```

### 4️⃣ **Commit Your Changes**

Use meaningful commit messages:

```bash
git commit -m "Add feature: improved string extension methods"
```

### 5️⃣ **Push and Open a Pull Request**

Push your changes to GitHub:

```bash
git push origin feature/my-new-feature
```

Then, open a pull request (PR) against the `main` or relevant feature branch.

---

## 🧪 **Testing Guidelines**

- All code must be covered by unit tests.

- Use [Shouldly](https://www.nuget.org/packages/Shouldly) for assertions.
- SeroGlint.DotNet uses xUnit for its testing framework
- If mocking is necessary, use [NSubstitute](https://www.nuget.org/packages/NSubstitute)
  - Be sure to cover positive and negative test cases
- Ensure tests pass before submitting a PR:

  ```bash
  dotnet test
  ```

---

## 📜 **Code Style Guide**

- Follow the official [.NET coding conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).
- Use `camelCase` for local variables and parameters.
- Use `PascalCase` for class names, properties, and methods.
- Use `_camelCase` for class fields
- Objects should own their own data, as such, please keep public properties to a minimum and leverage the [visitor pattern](https://refactoring.guru/design-patterns/visitor) for data contributions in the workflow
- Use XML documentation comments where necessary.

---

## 🗣️ **Need Help?**

If you have any questions or need guidance, feel free to [open an issue](https://github.com/Iterix-US/SeroGlint.DotNet/issues).

Thank you for helping improve **SeroGlint.DotNet**! 🚀
