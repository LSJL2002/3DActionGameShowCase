using System;

[Serializable]
public class VersionConfigData
{
    // 서버의 가장 최신 Addressables 콘텐츠 버전
    public string LatestContentVersion;

    // 이 콘텐츠를 로드할 수 있는 최소 앱 버전 (강제 업데이트용)
    public string MinimumAppVersion;

    // Addressables가 다운로드할 CDN 주소
    public string CDN_URL;
}