resource "kubernetes_namespace" "deo-namespace" {
  metadata {
    name = "deo"
  }
}

resource "kubernetes_deployment" "deo-backend" {
  metadata {
    namespace = "deo"
    name      = "deo-backend"
    labels = {
      app = "deo-backend"
    }
  }

  spec {
    replicas = 1
    selector {
      match_labels = {
        app = "deo-backend"
      }
    }
    template {
      metadata {
        labels = {
          app = "deo-backend"
        }
      }
      spec {
        container {
          image = "ghcr.io/jakobschlager/deo-webapp-backend:latest"
          name  = "deo-backend"
          args  = ["--urls", "http://*:3000"]
          port {
            container_port = 3000
          }
        }
      }
    }
  }
}

resource "kubernetes_service" "deo-backend-svc" {
  metadata {
    namespace = "deo"
    name      = "deo-backend-svc"
  }
  spec {
    selector = kubernetes_deployment.deo-backend.spec.0.template.0.metadata[0].labels
    port {
      protocol    = "TCP"
      port        = 3000
      target_port = 3000
    }
    cluster_ip = "None"
  }
}

resource "kubernetes_deployment" "deo-frontend" {
  metadata {
    namespace = "deo"
    name      = "deo-frontend"
    labels = {
      app = "deo-frontend"
    }
  }

  spec {
    replicas = 1
    selector {
      match_labels = {
        app = "deo-frontend"
      }
    }
    template {
      metadata {
        labels = {
          app = "deo-frontend"
        }
      }
      spec {
        container {
          image = "ghcr.io/jakobschlager/deo-webapp-frontend:latest"
          name  = "deo-frontend"
          args  = ["--urls", "http://*:2000"]
          port {
            container_port = 2000
          }
          env {
            name  = "DEO_BACKEND_URL"
            value = "http://deo-backend-svc:3000"
          }
        }
      }
    }
  }
}

resource "kubernetes_service" "deo-frontend-svc" {
  metadata {
    namespace = "deo"
    name      = "deo-frontend-svc"
  }
  spec {
    selector = kubernetes_deployment.deo-frontend.spec.0.template.0.metadata[0].labels
    port {
      protocol    = "TCP"
      port        = 2000
      target_port = 2000
    }
    type = "LoadBalancer"
  }
}
