FROM mcr.microsoft.com/dotnet/nightly/sdk:9.0

# Install any additional dependencies here
RUN apt-get update \
    && apt-get install -y git \
    && apt-get clean && rm -rf /var/lib/apt/lists/*

WORKDIR /workspace
