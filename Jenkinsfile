pipeline {
    agent {
        node {
            label 'master'
            customWorkspace 'workspace/BCFier'
        }
    }
    environment {
        GitHubAuthenticationToken = credentials('IabiBot_GitHub_AccessToken')
    }
    stages {
        stage ('Deploy') {
            steps {
                powershell './build.ps1 PublishGitHubRelease'
            }
        }
    }
    post {
        always {
            step([$class: 'Mailer',
                notifyEveryUnstableBuild: true,
                recipients: "dangl@iabi.eu",
                sendToIndividuals: true])
        }
    }
}