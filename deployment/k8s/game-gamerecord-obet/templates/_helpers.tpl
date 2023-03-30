{{/*
Expand the name of the chart.
*/}}
{{- define "game-gamerecord-obet.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "game-gamerecord-obet.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "game-gamerecord-obet.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "game-gamerecord-obet.labels" -}}
helm.sh/chart: {{ include "game-gamerecord-obet.chart" . }}
{{ include "game-gamerecord-obet.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "game-gamerecord-obet.selectorLabels" -}}
app.kubernetes.io/name: {{ include "game-gamerecord-obet.name" . }}
app: {{ include "game-gamerecord-obet.name" . }}
tgs.kind: game
tgs.game.kind: gamerecord
tgs.gamerecord.kind: service
tgs.gameprovider.kind: obet
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "game-gamerecord-obet.serviceAccountName" -}}
{{- if .Values.serviceAccount.create }}
{{- default (include "game-gamerecord-obet.fullname" .) .Values.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.serviceAccount.name }}
{{- end }}
{{- end }}
